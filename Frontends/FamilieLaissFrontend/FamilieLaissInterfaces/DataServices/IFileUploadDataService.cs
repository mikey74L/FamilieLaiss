using FamilieLaissInterfaces.Models;

namespace FamilieLaissInterfaces.DataServices;

public interface IFileUploadDataService
{
    Task<IApiResult<long>> GetIdForUploadFromServerAsync();

    Task<IApiResult<bool>> UploadChunkPictureAsync(string fileNameTarget, long chunkNumber, int bytesRead,
        byte[] buffer);

    Task<IApiResult<bool>> FinishUploadChunkPictureAsync(string fileNameTarget, long lastChunkNumber,
        string originalFilename);

    Task<IApiResult<bool>> UploadChunkVideoAsync(string fileNameTarget, long chunkNumber, int bytesRead, byte[] buffer);

    Task<IApiResult<bool>> FinishUploadChunkVideoAsync(string fileNameTarget, long lastChunkNumber,
        string originalFilename);
}