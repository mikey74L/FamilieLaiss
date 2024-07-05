namespace Upload.API.Interfaces;

public interface IUniqueIdentifierGenerator
{
    /// <summary>
    /// Get the next unique Upload-ID
    /// </summary>
    /// <returns>The generated Upload-ID</returns>
    Task<long> GetNextUploadIDAsync();
}