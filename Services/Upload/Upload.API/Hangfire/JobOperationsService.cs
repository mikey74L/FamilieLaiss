using Hangfire;
using Upload.API.Enums;

namespace Upload.API.Hangfire;

/// <summary>
/// Hangfire job operations service
/// </summary>
public class JobOperationsService : iJobOperations
{
    #region Upload
    /// <summary>
    /// Creates from a list of chuncks a file and move this file to the target directory.
    /// After this the chucks would be deleted.
    /// </summary>
    /// <param name="tempUploadFolder">Temporary Upload-Folder.</param>
    /// <param name="uploadFolder">Upload-Folder (Targetdirectory).</param>
    /// <param name="destinationFilename">Filename for target file.</param>
    /// <param name="lastChunkNumber">The last chunk number used for the upload file</param>
    /// <param name="waitTimeForJob">Wait time before the job will be executed.</param>
    /// <param name="deleteAlreadyExisting">Delete an already existing file with the same name in target directory.</param>
    /// <returns>Job-ID</returns>
    public string UploadMakeFileFromChunks(string tempUploadFolder, string uploadFolder, string destinationFilename, long lastChunkNumber, bool deleteAlreadyExisting, int waitTimeForJob)
    {
        //Einstellen des Jobs mit 15 Sekunden Verzögerung
        return BackgroundJob.Schedule<JobExecuter>(x => x.UploadMakeFileFromChunks(tempUploadFolder, uploadFolder, destinationFilename,
            lastChunkNumber, deleteAlreadyExisting),
            TimeSpan.FromSeconds(waitTimeForJob));
    }

    /// <summary>
    /// Creates a job to make an entry in uploadpicture or uploadvideo. 
    /// This job will be created in conjunction to the job for creating a file from chuncks.
    /// </summary>
    /// <param name="JobIDParent">Job ID for parent job.</param>
    /// <param name="uploadType">The type of upload file.</param>
    /// <param name="ID">ID of upload item.</param>
    /// <param name="originalName">The original filename for the uploaded file.</param>
    /// <param name="userName">The user name for user account (only needed when portrait is uploaded)</param>
    /// <returns>Job-ID</returns>
    public string WriteToUploadQueue(string JobIDParent, UploadType uploadType, long ID, string originalName, string userName)
    {
        //Einstellen des Jobs in Abhängigkeit zu Parent-Job
        return BackgroundJob.ContinueJobWith<JobExecuter>(JobIDParent, x => x.WriteToUploadQueue(uploadType, ID, originalName, userName));
    }
    #endregion
}
