using Upload.API.Enums;

namespace Upload.API.Hangfire;

/// <summary>
/// Interface for Job-Operations that should be executed in the background
/// </summary>
public interface iJobOperations
{
    /// <summary>
    /// Creates from a list of chuncks a file and move this file to the target directory.
    /// After this the chucks would be deleted.
    /// </summary>
    /// <param name="tempUploadFolder">Temporary Upload-Folder.</param>
    /// <param name="uploadFolder">Upload-Folder (Targetdirectory).</param>
    /// <param name="destinationFilename">Filename for target file.</param>
    /// <param name="lastChunkNumber">The last chunk number used for the uploaded file</param>
    /// <param name="waitTimeForJob">Wait time before the job will be executed.</param>
    /// <param name="deleteAlreadyExisting">Delete an already existing file with the same name in target directory.</param>
    /// <returns>Job-ID</returns>
    string UploadMakeFileFromChunks(string tempUploadFolder, string uploadFolder, string destinationFilename, long lastChunkNumber, bool deleteAlreadyExisting, int waitTimeForJob);

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
    string WriteToUploadQueue(string JobIDParent, UploadType uploadType, long ID, string originalName, string userName);
}
