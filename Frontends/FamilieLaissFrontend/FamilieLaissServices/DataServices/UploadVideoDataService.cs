using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissMappingExtensions.UploadVideo;
using StrawberryShake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilieLaissServices.DataServices;

public class UploadVideoDataService(
    IFamilieLaissClient familieLaissClient,
    IServiceProvider serviceProvider)
    : BaseDataService(familieLaissClient), IUploadVideoDataService
{
    #region Query

    public async Task<IApiResult<IEnumerable<IUploadVideoModel>>> GetUploadVideosForChooseViewAsync()
    {
        try
        {
            var response = await Client.GetUploadVideosForChooseView.ExecuteAsync();

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.UploadVideos.Map());
            }

            return CreateApiResultForError<IEnumerable<IUploadVideoModel>>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<IEnumerable<IUploadVideoModel>>(ex);
        }
    }

    public async Task<IApiResult<IEnumerable<IUploadVideoModel>>> GetUploadVideosForUploadViewAsync(
        IReadOnlyList<UploadVideoSortInput> sortCriterias, UploadVideoFilterInput? filterCriteria)
    {
        try
        {
            var response = await Client.GetUploadVideosForUploadView.ExecuteAsync(sortCriterias, filterCriteria);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.UploadVideos.Map(serviceProvider));
            }

            return CreateApiResultForError<IEnumerable<IUploadVideoModel>>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<IEnumerable<IUploadVideoModel>>(ex);
        }
    }

    #endregion

    #region CRUD

    public async Task<IApiResult<IUploadVideoModel?>> DeleteUploadVideoAsync(IUploadVideoModel model)
    {
        try
        {
            var response = await Client.DeleteUploadVideo.ExecuteAsync(model.Id);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.DeleteUploadVideo.UploadVideo.Map());
            }

            return CreateApiResultForError<IUploadVideoModel?>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<IUploadVideoModel?>(ex);
        }
    }

    public async Task<IApiResult<IEnumerable<IUploadVideoModel>>> DeleteAllUploadVideosAsync(
        IEnumerable<IUploadVideoModel> modelsToDelete)
    {
        try
        {
            var response = await Client.DeleteAllUploadVideos.ExecuteAsync(modelsToDelete.Select(x => x.Id).ToList());

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.DeleteAllUploadVideos.UploadVideos.Map());
            }

            return CreateApiResultForError<IEnumerable<IUploadVideoModel>>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<IEnumerable<IUploadVideoModel>>(ex);
        }
    }

    public async Task<IApiResult<int>> GetUploadVideoCount()
    {
        try
        {
            var response = await Client.GetUploadVideoCount.ExecuteAsync();

            if (response.Data is not null)
            {
                return CreateApiResult(response.Data.UploadVideoCount);
            }

            return CreateApiResultForError<int>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<int>(ex);
        }
    }

    #endregion
}