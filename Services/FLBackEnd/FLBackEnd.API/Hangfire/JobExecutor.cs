using DomainHelper.Interfaces;
using FamilieLaissMassTransitDefinitions.Commands;
using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using FLBackEnd.API.Enums;
using FLBackEnd.Domain.Entities;
using HotChocolate.Subscriptions;
using MassTransit;
using UploadType = FLBackEnd.API.Enums.UploadType;

namespace FLBackEnd.API.Hangfire;

/// <summary>
/// This class is holds the job methods for Hangfire that will be executed
/// </summary>
public class JobExecutor(
    iUnitOfWork unitOfWork,
    IBus bus,
    ITopicEventSender eventSender,
    ILogger<JobExecutor> logger)
{
    #region Upload

    /// <summary>
    /// Creates a file from a list of chunks and then moves it to the destination directory.
    /// Afterward, the chunks are deleted.
    /// </summary>
    /// <param name="tempUploadFolder">Temporary upload folder</param>
    /// <param name="uploadFolder">Upload folder (destination directory)</param>
    /// <param name="destinationFilename">Filename for the destination file</param>
    /// <param name="lastChunkNumber">The last chunk number used for the upload file</param>
    /// <param name="deleteAlreadyExisting">Deletes an already existing file in the destination directory with the same name</param>
    public void UploadMakeFileFromChunks(string tempUploadFolder, string uploadFolder, string destinationFilename,
        long lastChunkNumber, bool deleteAlreadyExisting)
    {
        logger.LogInformation("Action called with following parameters:");
        logger.LogDebug($"Temporary upload folder: {tempUploadFolder}");
        logger.LogDebug($"Upload-Folder          : {uploadFolder}");
        logger.LogDebug($"Destination filename   : {destinationFilename}");
        logger.LogDebug($"Last chunk number      : {lastChunkNumber}");
        logger.LogDebug($"Delete already existing: {deleteAlreadyExisting}");

        try
        {
            logger.LogDebug($"Create directory \"{uploadFolder}\" if not exists");
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            logger.LogDebug(
                $"Create and open destination stream for \"{System.IO.Path.Combine(tempUploadFolder, destinationFilename)}\"");
            var destStream = File.Create(System.IO.Path.Combine(tempUploadFolder, destinationFilename));
            logger.LogDebug("Adding all chunks to destination stream");
            List<string> listOfChunkFilenames = [];
            for (var i = 1; i <= lastChunkNumber; i++)
            {
                var filenameSource = System.IO.Path.Combine(tempUploadFolder,
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
                    $"Check is file \"{System.IO.Path.Combine(uploadFolder, destinationFilename)}\" already exists");
                if (File.Exists(System.IO.Path.Combine(uploadFolder, destinationFilename)))
                {
                    logger.LogDebug($"Delete file \"{System.IO.Path.Combine(uploadFolder, destinationFilename)}\"");
                    File.Delete(System.IO.Path.Combine(uploadFolder, destinationFilename));
                }
            }

            logger.LogDebug(
                $"Move file from \"{System.IO.Path.Combine(tempUploadFolder, destinationFilename)}\" to \"{System.IO.Path.Combine(uploadFolder, destinationFilename)}\"");
            File.Move(System.IO.Path.Combine(tempUploadFolder, destinationFilename),
                System.IO.Path.Combine(uploadFolder, destinationFilename));

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
            UploadPicture? newPicture = null;
            UploadVideo? newVideo = null;
            PictureConvertStatus? newPictureStatus = null;
            VideoConvertStatus? newVideoStatus = null;
            switch (uploadType)
            {
                case UploadType.Video:
                {
                    logger.LogDebug("Upload is a video");

                    logger.LogDebug("Create new entity");
                    newVideo = new UploadVideo(id, originalName);

                    logger.LogDebug("Get repository for upload video");
                    var repositoryUploadVideo = unitOfWork.GetRepository<UploadVideo>();

                    logger.LogDebug("Get repository for ConvertStatus");
                    var repositoryConvertStatus = unitOfWork.GetRepository<VideoConvertStatus>();

                    logger.LogDebug("Adding new ConvertStatus domain model to repository");
                    newVideoStatus = new(newVideo);
                    await repositoryConvertStatus.AddAsync(newVideoStatus);

                    logger.LogDebug("Adding entity to store");
                    await repositoryUploadVideo.AddAsync(newVideo);
                    break;
                }
                case UploadType.Picture:
                {
                    logger.LogDebug("Upload is a picture");

                    logger.LogDebug("Create new upload picture entity");
                    newPicture = new UploadPicture(id, originalName);

                    logger.LogDebug("Get repository for upload picture");
                    var repositoryUploadPicture = unitOfWork.GetRepository<UploadPicture>();

                    logger.LogDebug("Get repository for ConvertStatus");
                    var repositoryConvertStatus = unitOfWork.GetRepository<PictureConvertStatus>();

                    logger.LogDebug("Adding new ConvertStatus domain model to repository");
                    newPictureStatus = new(newPicture);
                    await repositoryConvertStatus.AddAsync(newPictureStatus);

                    logger.LogDebug("Adding entity to store");
                    await repositoryUploadPicture.AddAsync(newPicture);
                    break;
                }
                case UploadType.Portrait:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(uploadType), uploadType, null);
            }

            ////Es handelt sich um ein Portrait
            ////Beim Portrait kann es auch sein, dass es schon einen Eintrag gibt und das Portrait-Bild
            ////ausgetauscht werden soll. In diesem Fall muss der bestehende Eintrag gelöscht, und ein
            ////neuer Eintrag erstellt werden
            //if (uploadType == enUploadType.Portrait)
            //{
            //    //Logging
            //    _Logger.LogDebug("Upload is a portrait");

            //    //Erstellen der Entity
            //    _Logger.LogDebug("Create new entity");
            //    var NewEntity = new UploadPortrait(ID, userName);

            //    //Erstellen des Repository
            //    _Logger.LogDebug("Get repository");
            //    var Repository = _UnitOfWork.GetRepository<UploadPortrait>();

            //    //Überprüfen ob es schon einen Eintrag gibt
            //    _Logger.LogDebug("Check if already a entity exists for the user");
            //    var FoundItem = (await Repository.GetAll(x => x.UserName == userName)).FirstOrDefault();

            //    //Wenn ein Element gefunden wurde, muss dieses zuerst gelöscht werden
            //    if (FoundItem != null)
            //    {
            //        _Logger.LogDebug("A entity for the given user already exists. Delete existing entity before adding new one.");
            //        Repository.Delete(FoundItem);
            //    }

            //    //Hinzufügen der Entity zum Store
            //    _Logger.LogDebug("Adding entity to store");
            //    await Repository.AddAsync(NewEntity);
            //}

            logger.LogDebug("Saving changes");
            await unitOfWork.SaveChangesAsync();

            switch (uploadType)
            {
                case UploadType.Picture:
                    logger.LogDebug("Sending event to subscribers");
                    await eventSender.SendAsync(nameof(SubscriptionType.PictureConvertStatusWaiting), newPictureStatus);

                    logger.LogDebug("Publish picture uploaded event over service bus");
                    if (newPicture is not null && newPictureStatus is not null)
                    {
                        await bus.Send<IMassConvertPictureCmd>(new MassConvertPictureCmd()
                        {
                            Id = newPicture.Id, ConvertStatusId = newPictureStatus.Id,
                            OriginalName = newPicture.Filename
                        });
                    }

                    break;
                case UploadType.Video:
                    logger.LogDebug("Sending event to subscribers");
                    await eventSender.SendAsync(nameof(SubscriptionType.VideoConvertStatusWaiting), newVideoStatus);

                    logger.LogDebug("Publish video uploaded event over service bus");
                    if (newVideo is not null && newVideoStatus is not null)
                    {
                        await bus.Send<IMassConvertVideoCmd>(new MassConvertVideoCmd()
                        {
                            Id = newVideo.Id,
                            ConvertStatusId = newVideoStatus.Id,
                            OriginalName = newVideo.Filename
                        });
                    }

                    break;
                case UploadType.Portrait:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(uploadType), uploadType, null);
            }
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Unexpected error occurred");
            throw;
        }
    }

    #endregion
}