using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Models;
using StrawberryShake;
using System;
using System.Threading.Tasks;

namespace FamilieLaissServices.DataServices;

public class FileUploadDataService(IFamilieLaissClient familieLaissClient) : BaseDataService(familieLaissClient), IFileUploadDataService
{
    #region General

    public async Task<IApiResult<long>> GetIdForUploadFromServerAsync()
    {
        try
        {
            var response = await Client.GetNextUploadId.ExecuteAsync();

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.NextUploadId);
            }

            return CreateApiResultForError<long>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<long>(ex);
        }
    }

    #endregion

    #region Picture Upload

    public async Task<IApiResult<bool>> UploadChunkPictureAsync(string fileNameTarget, long chunkNumber, int bytesRead,
        byte[] buffer)
    {
        try
        {
            var response =
                await Client.AddPictureUploadChunk.ExecuteAsync(chunkNumber, bytesRead, Convert.ToBase64String(buffer),
                    fileNameTarget);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.AddPictureUploadChunk.Status);
            }

            return CreateApiResultForError<bool>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<bool>(ex);
        }
    }

    public async Task<IApiResult<bool>> FinishUploadChunkPictureAsync(string fileNameTarget, long lastChunkNumber,
        string originalFilename)
    {
        try
        {
            var response =
                await Client.PictureUploadFinish.ExecuteAsync(lastChunkNumber, fileNameTarget, originalFilename);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.PictureUploadFinish.Status);
            }

            return CreateApiResultForError<bool>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<bool>(ex);
        }
    }

    #endregion

    #region Video Upload

    public async Task<IApiResult<bool>> UploadChunkVideoAsync(string fileNameTarget, long chunkNumber, int bytesRead,
        byte[] buffer)
    {
        try
        {
            var response =
                await Client.AddVideoUploadChunk.ExecuteAsync(chunkNumber, bytesRead, Convert.ToBase64String(buffer),
                    fileNameTarget);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.AddVideoUploadChunk.Status);
            }

            return CreateApiResultForError<bool>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<bool>(ex);
        }
    }

    public async Task<IApiResult<bool>> FinishUploadChunkVideoAsync(string fileNameTarget, long lastChunkNumber,
        string originalFilename)
    {
        try
        {
            var response =
                await Client.VideoUploadFinish.ExecuteAsync(lastChunkNumber, fileNameTarget, originalFilename);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.VideoUploadFinish.Status);
            }

            return CreateApiResultForError<bool>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<bool>(ex);
        }
    }

    #endregion
}