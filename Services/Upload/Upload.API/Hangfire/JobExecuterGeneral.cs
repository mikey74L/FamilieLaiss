using DomainHelper.Interfaces;
using Upload.API.Interfaces;
using Upload.Domain.Entities;
using UploadType = Upload.API.Enums.UploadType;

namespace Upload.API.Hangfire;

/// <summary>
/// This class is holds the job methods for Hangfire that will be executed
/// </summary>
public class JobExecutorGeneral(
    iUnitOfWork unitOfWork,
    IFolderHelperService folderHelperService,
    ILogger<JobExecutorGeneral> logger)
{
    #region Upload

    /// <summary>
    /// Creates a file from a list of chunks and then moves it to the destination directory.
    /// Afterward, the chunks are deleted.
    /// </summary>
    /// <param name="destinationFilename">Filename for the destination file</param>
    /// <param name="lastChunkNumber">The last chunk number used for the upload file</param>
    /// <param name="deleteAlreadyExisting">Deletes an already existing file in the destination directory with the same name</param>
    public void UploadMakeFileFromChunks(string destinationFilename,
        long lastChunkNumber, bool deleteAlreadyExisting)
    {
        logger.LogInformation("Action called with following parameters:");
        logger.LogDebug($"Destination filename   : {destinationFilename}");
        logger.LogDebug($"Last chunk number      : {lastChunkNumber}");
        logger.LogDebug($"Delete already existing: {deleteAlreadyExisting}");

        try
        {
            var targetPath = folderHelperService.GetFolderNameForUploadPicture();
            logger.LogDebug($"Create directory \"{targetPath}\" if not exists");
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            logger.LogDebug(
                $"Create and open destination stream for \"{folderHelperService.GetFullFilenameForTempUploadPicture(destinationFilename)}\"");
            var destStream = File.Create(folderHelperService.GetFullFilenameForTempUploadPicture(destinationFilename));
            logger.LogDebug("Adding all chunks to destination stream");
            List<string> listOfChunkFilenames = [];
            for (var i = 1; i <= lastChunkNumber; i++)
            {
                var filenameSource = folderHelperService.GetFullFilenameForTempUploadPicture(
                    $"{System.IO.Path.GetFileNameWithoutExtension(destinationFilename)}-{i}{System.IO.Path.GetExtension(destinationFilename)}");
                var sourceStream = File.OpenRead(filenameSource);
                listOfChunkFilenames.Add(filenameSource);
                sourceStream.CopyTo(destStream);
                sourceStream.Close();
                sourceStream.Dispose();
            }

            logger.LogDebug("Close destination stream");
            destStream.Close();
            logger.LogDebug("Dispose destination stream");
            destStream.Dispose();

            if (deleteAlreadyExisting)
            {
                logger.LogDebug(
                    $"Check is file \"{System.IO.Path.Combine(targetPath, destinationFilename)}\" already exists");
                if (File.Exists(System.IO.Path.Combine(targetPath, destinationFilename)))
                {
                    logger.LogDebug($"Delete file \"{System.IO.Path.Combine(targetPath, destinationFilename)}\"");
                    File.Delete(System.IO.Path.Combine(targetPath, destinationFilename));
                }
            }

            var source = folderHelperService.GetFullFilenameForTempUploadPicture(destinationFilename);
            var destination = folderHelperService.GetFullFilenameForUploadPicture(destinationFilename);
            logger.LogDebug($"Move file from \"{source}\" to \"{destination}\"");
            File.Move(source,destination);

            logger.LogDebug("Delete all chunks");
            foreach (var filename in listOfChunkFilenames)
            {
                try
                {
                    File.Delete(filename);
                }
                catch (Exception ex)
                {
                    logger.LogCritical(ex, "Not all chunks have been deleted");
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Unexpected error occurred");
            throw;
        }
    }

    /// <summary>
    /// Add an entry to the upload queue. This entry will trigger the Picture or Video Converter Service.
    /// </summary>
    /// <param name="uploadType">The type of upload</param>
    /// <param name="id">The ID of the upload item</param>
    /// <param name="originalName">The original filename of the uploaded file</param>
    /// <param name="userName">The username for the user account (only needed when uploading a portrait)</param>
    public async Task WriteToUploadQueue(UploadType uploadType, long id, string originalName,
        string userName)
    {
        logger.LogInformation("Action called with following parameters:");
        logger.LogDebug($"Upload-Type  : {uploadType}");
        logger.LogDebug($"ID           : {id}");
        logger.LogDebug($"Original-Name: {originalName}");
        if (!string.IsNullOrEmpty(userName))
        {
            logger.LogDebug($"User-Name    : {userName.Substring(1, 3)}");
        }

        try
        {
            switch (uploadType)
            {
                case UploadType.Video:
                    {
                        logger.LogDebug("Upload is a video");

                        logger.LogDebug("Create new entity");
                        var newVideo = new UploadVideo(id, originalName);

                        logger.LogDebug("Get repository for upload video");
                        var repositoryUploadVideo = unitOfWork.GetRepository<UploadVideo>();

                        logger.LogDebug("Adding entity to store");
                        await repositoryUploadVideo.AddAsync(newVideo);
                        break;
                    }
                case UploadType.Picture:
                    {
                        logger.LogDebug("Upload is a picture");

                        logger.LogDebug("Create new upload picture entity");
                        var newPicture = new UploadPicture(id, originalName);

                        logger.LogDebug("Get repository for upload picture");
                        var repositoryUploadPicture = unitOfWork.GetRepository<UploadPicture>();

                        logger.LogDebug("Adding entity to store");
                        await repositoryUploadPicture.AddAsync(newPicture);
                        
                        logger.LogDebug("Create new picture convert status entity");
                        var newPictureConvertStatus = new PictureConvertStatus(newPicture);
                        
                        logger.LogDebug("Get repository for upload picture");
                        var repositoryPictureConvertStatus = unitOfWork.GetRepository<PictureConvertStatus>();
                        
                        logger.LogDebug("Adding entity to store");
                        await repositoryPictureConvertStatus.AddAsync(newPictureConvertStatus);
                        break;
                    }
                case UploadType.Portrait:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(uploadType), uploadType, null);
            }

            logger.LogDebug("Saving changes");
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Unexpected error occurred");
            throw;
        }
    }

    #endregion
}