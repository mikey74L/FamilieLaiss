namespace Upload.API.Hangfire;

/// <summary>
/// Interface for Job-Operations that should be executed in the background
/// </summary>
public interface IJobOperations
{
    /// <summary>
    /// Creates from a list of chunks a file and move this file to the target directory.
    /// After this the chucks would be deleted.
    /// </summary>
    /// <param name="destinationFilename">Filename for target file.</param>
    /// <param name="lastChunkNumber">The last chunk number used for the uploaded file</param>
    /// <param name="waitTimeForJob">Wait time before the job will be executed.</param>
    /// <param name="deleteAlreadyExisting">Delete an already existing file with the same name in target directory.</param>
    /// <returns>Job-ID</returns>
    string UploadMakeFileFromChunks(string destinationFilename,
        long lastChunkNumber, bool deleteAlreadyExisting, int waitTimeForJob);

    /// <summary>
    /// Extracts the picture information from the uploaded picture.
    /// </summary>
    /// <param name="jobIdParent">Job ID for parent job.</param>
    /// <param name="destinationFilename">Filename for target file</param>
    /// <param name="id">ID of upload item.</param>
    /// <returns>Job-ID</returns>
    string ExtractPictureInfo(string jobIdParent, long id, string destinationFilename);
    
    /// <summary>
    /// Extracts the picture metadata from the uploaded picture (Exif).
    /// </summary>
    /// <param name="jobIdParent">Job ID for parent job.</param>
    /// <param name="destinationFilename">Filename for target file</param>
    /// <param name="id">ID of upload item.</param>
    /// <returns>Job-ID</returns>
    string ExtractPictureMetadata(string jobIdParent, long id, string destinationFilename);

    /// <summary>
    /// Converts the uploaded picture to different formats needed for web ui.
    /// And adds the converted files to blob storage
    /// </summary>
    /// <param name="jobIdParent">Job ID for parent job.</param>
    /// <param name="destinationFilename">Filename for target file</param>
    /// <param name="id">ID of upload item.</param>
    /// <returns>Job-ID</returns>
    string ConvertPicture(string jobIdParent, long id, string destinationFilename);
    
    /// <summary>
    /// Uploads the converted picture and the original picture to blob storage.
    /// </summary>
    /// <param name="jobIdParent">Job ID for parent job.</param>
    /// <param name="destinationFilename">Filename for target file</param>
    /// <param name="id">ID of upload item.</param>
    /// <returns>Job-ID</returns>
    string UploadPictureToBlobStorage(string jobIdParent, long id, string destinationFilename);
    
    /// <summary>
    /// Deletes the converted and original picture from physical drive 
    /// </summary>
    /// <param name="jobIdParent">Job ID for parent job</param>
    /// <param name="id">ID of upload item</param>
    /// <param name="destinationFilename">Filename for target file</param>
    /// <returns></returns>
    string DeleteFilesFromPhysicalDrive(string jobIdParent, long id, string destinationFilename);
    
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
    string WriteToUploadQueue(string jobIdParent, Enums.UploadType uploadType, long id,
        string originalName, string userName);
}