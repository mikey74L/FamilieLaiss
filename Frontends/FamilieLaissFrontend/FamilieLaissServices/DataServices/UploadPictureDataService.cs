using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissMappingExtensions.UploadPicture;
using FamilieLaissModels.Models.UploadPicture;
using StrawberryShake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilieLaissServices.DataServices;

public class UploadPictureDataService(IFamilieLaissClient familieLaissClient)
    : BaseDataService(familieLaissClient), IUploadPictureDataService
{
    #region Query
    public async Task<IApiResult<IEnumerable<IUploadPictureModel>>> GetUploadPicturesForChooseViewAsync()
    {
        try
        {
            var response = await Client.GetUploadPicturesForChooseView.ExecuteAsync();

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.UploadPictures.Map());
            }

            return CreateApiResultForError<IEnumerable<IUploadPictureModel>>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<IEnumerable<IUploadPictureModel>>(ex);
        }
    }


    public async Task<IApiResult<IEnumerable<IUploadPictureModel>>> GetUploadPicturesForUploadViewAsync(IReadOnlyList<UploadPictureSortInput> sortCriterias, UploadPictureFilterInput? filterCriteria)
    {
        try
        {
            var response = await Client.GetUploadPicturesForUploadView.ExecuteAsync(sortCriterias, filterCriteria);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.UploadPictures.Map());
            }

            return CreateApiResultForError<IEnumerable<IUploadPictureModel>>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<IEnumerable<IUploadPictureModel>>(ex);
        }
    }

    public async Task<IUploadPictureExifInfoFilterData?> GetUploadPictureExifInfoFilterData()
    {
        try
        {
            UploadPictureExifInfoFilterData? result = new();

            var filterDataRaw = await Client.GetUploadPictureExifInfoFilterData.ExecuteAsync();

            if (filterDataRaw.Data is not null)
            {
                result.Makes.AddRange(filterDataRaw.Data.UploadPictureExifInfoFilterData.Make);
                result.Models.AddRange(filterDataRaw.Data.UploadPictureExifInfoFilterData.Model);
                result.ISOSensitivities.AddRange(filterDataRaw.Data.UploadPictureExifInfoFilterData.IsoSensitivities);
                result.FNumbers.AddRange(filterDataRaw.Data.UploadPictureExifInfoFilterData.FNumbers);
                result.ExposureTimes.AddRange(filterDataRaw.Data.UploadPictureExifInfoFilterData.ExposureTimes);
                result.ShutterSpeeds.AddRange(filterDataRaw.Data.UploadPictureExifInfoFilterData.ShutterSpeeds);
                result.FocalLengths.AddRange(filterDataRaw.Data.UploadPictureExifInfoFilterData.FocalLengths);
            }

            return result;
        }
        catch
        {
            return null;
        }
    }
    #endregion

    #region CRUD

    public async Task<IApiResult<IUploadPictureModel?>> DeleteUploadPictureAsync(IUploadPictureModel model)
    {
        try
        {
            var response = await Client.DeleteUploadPicture.ExecuteAsync(model.Id);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.DeleteUploadPicture.UploadPicture.Map());
            }

            return CreateApiResultForError<IUploadPictureModel?>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<IUploadPictureModel?>(ex);
        }
    }

    public async Task<IApiResult<IEnumerable<IUploadPictureModel>>> DeleteAllUploadPicturesAsync(IEnumerable<IUploadPictureModel> modelsToDelete)
    {
        try
        {
            var response = await Client.DeleteAllUploadPictures.ExecuteAsync(modelsToDelete.Select(x => x.Id).ToList());

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.DeleteAllUploadPictures.UploadPictures.Map());
            }

            return CreateApiResultForError<IEnumerable<IUploadPictureModel>>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<IEnumerable<IUploadPictureModel>>(ex);
        }
    }

    public async Task<IApiResult<int>> GetUploadPictureCount()
    {
        try
        {
            var response = await Client.GetUploadPictureCount.ExecuteAsync();

            if (response.Data is not null)
            {
                return CreateApiResult(response.Data.UploadPictureCount);
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