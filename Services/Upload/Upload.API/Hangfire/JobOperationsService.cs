using Hangfire;

namespace Upload.API.Hangfire;

/// <summary>
/// Hangfire job operations service
/// </summary>
public class JobOperationsService : IJobOperations
{
    #region Upload

    /// <summary>
    /// Creates from a list of chunks a file and move this file to the target directory.
    /// After this the chucks would be deleted.
    /// </summary>
    /// <param name="destinationFilename">Filename for target file.</param>
    /// <param name="lastChunkNumber">The last chunk number used for the upload file</param>
    /// <param name="waitTimeForJob">Wait time before the job will be executed.</param>
    /// <param name="deleteAlreadyExisting">Delete an already existing file with the same name in target directory.</param>
    /// <returns>Job-ID</returns>
    public string UploadMakeFileFromChunks(string destinationFilename,
        long lastChunkNumber, bool deleteAlreadyExisting, int waitTimeForJob)
    {
        return BackgroundJob.Schedule<JobExecutorGeneral>(x => x.UploadMakeFileFromChunks(
                destinationFilename,
                lastChunkNumber, deleteAlreadyExisting),
            TimeSpan.FromSeconds(waitTimeForJob));
    }
 
    /// <inheritdoc />
    public string ExtractPictureInfo(string jobIdParent, long id, string destinationFilename)
    {
        return BackgroundJob.ContinueJobWith<JobExecutorUploadPicture>(jobIdParent,
            x => x.ExtractPictureInfo(id, destinationFilename));
    }

    /// <inheritdoc />
    public string ExtractPictureMetadata(string jobIdParent, long id, string destinationFilename)
    {
        return BackgroundJob.ContinueJobWith<JobExecutorUploadPicture>(jobIdParent,
            x => x.ExtractPictureMetadata(id, destinationFilename));
    }

    /// <inheritdoc />
    public string ConvertPicture(string jobIdParent, long id, string destinationFilename)
    {
        return BackgroundJob.ContinueJobWith<JobExecutorUploadPicture>(jobIdParent,
            x => x.ConvertPicture(id, destinationFilename));
    }

    /// <inheritdoc />
    public string UploadPictureToBlobStorage(string jobIdParent, long id, string destinationFilename)
    {
        return BackgroundJob.ContinueJobWith<JobExecutorUploadPicture>(jobIdParent,
            x => x.UploadPictureToBlobStorage(id, destinationFilename));
    }

    /// <inheritdoc />
    public string DeleteFilesFromPhysicalDrive(string jobIdParent, long id, string destinationFilename)
    {
        return BackgroundJob.ContinueJobWith<JobExecutorUploadPicture>(jobIdParent,
            x => x.DeleteFilesFromPhysicalDrive(id, destinationFilename));
    }

    /// <summary>
    /// Creates a job to make an entry in upload picture or upload video. 
    /// This job will be created in conjunction to the job for creating a file from chunks.
    /// </summary>
    /// <param name="jobIdParent">Job ID for parent job.</param>
    /// <param name="uploadType">The type of upload file.</param>
    /// <param name="id">ID of upload item.</param>
    /// <param name="originalName">The original filename for the uploaded file.</param>
    /// <param name="userName">The username for user account (only needed when portrait is uploaded)</param>
    /// <returns>Job-ID</returns>
    public string WriteToUploadQueue(string jobIdParent, Enums.UploadType uploadType, long id,
        string originalName,
        string userName)
    {
        return BackgroundJob.ContinueJobWith<JobExecutorGeneral>(jobIdParent,
            x => x.WriteToUploadQueue(uploadType, id, originalName, userName));
    }

    #endregion
}