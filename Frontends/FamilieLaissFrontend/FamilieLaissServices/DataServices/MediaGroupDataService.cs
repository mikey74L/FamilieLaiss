using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissMappingExtensions.MediaGroup;
using StrawberryShake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilieLaissServices.DataServices;

public class MediaGroupDataService(IFamilieLaissClient familieLaissClient)
    : BaseDataService(familieLaissClient), IMediaGroupDataService
{
    #region Validation

    public async Task<IApiResult<bool>> EnglishMediaGroupNameExistsAsync(long id, string? englishName)
    {
        try
        {
            var response = await Client.EnglishMediaGroupNameExists.ExecuteAsync(englishName ?? "", id);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.EnglishMediaGroupNameExists);
            }

            return CreateApiResultForError<bool>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<bool>(ex);
        }
    }

    public async Task<IApiResult<bool>> GermanMediaGroupNameExistsAsync(long id, string? germanName)
    {
        try
        {
            var response = await Client.GermanMediaGroupNameExists.ExecuteAsync(germanName ?? "", id);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.GermanMediaGroupNameExists);
            }

            return CreateApiResultForError<bool>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<bool>(ex);
        }
    }

    #endregion

    #region Query
    public async Task<IApiResult<IEnumerable<IMediaGroupModel>>> GetAllMediaGroupsAsync()
    {
        try
        {
            var response = await Client.GetAllMediaGroups.ExecuteAsync();

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.MediaGroups.Map());
            }

            return CreateApiResultForError<IEnumerable<IMediaGroupModel>>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<IEnumerable<IMediaGroupModel>>(ex);
        }
    }

    public async Task<IApiResult<IMediaGroupModel>> GetMediaGroupAsync(long id)
    {
        try
        {
            var response = await Client.GetMediaGroup.ExecuteAsync(id);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.MediaGroups.First().Map());
            }

            return CreateApiResultForError<IMediaGroupModel>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<IMediaGroupModel>(ex);
        }
    }

    #endregion

    #region CRUD

    public async Task<IApiResult> AddMediaGroupAsync(IMediaGroupModel model)
    {
        if (model.NameGerman is null)
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model.NameEnglish is null)
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model.DescriptionGerman is null)
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model.DescriptionEnglish is null)
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model.EventDate is null)
        {
            return CreateSimpleApiResultForBadRequest();
        }

        try
        {
            var response =
                await Client.AddMediaGroup.ExecuteAsync(model.NameGerman, model.NameEnglish,
                                                        model.DescriptionGerman, model.DescriptionEnglish,
                                                        model.EventDate.Value);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                model.Id = response.Data.AddMediaGroup.MediaGroup.Id;
                model.CreateDate = response.Data.AddMediaGroup.MediaGroup.CreateDate;

                return CreateSimpleApiResult();
            }

            return CreateSimpleApiResultForError(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateSimpleApiResultForCommunicationError(ex);
        }
    }

    public async Task<IApiResult> UpdateMediaGroupAsync(IMediaGroupModel model)
    {
        if (model.NameGerman is null)
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model.NameEnglish is null)
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model.DescriptionGerman is null)
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model.DescriptionEnglish is null)
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model.EventDate is null)
        {
            return CreateSimpleApiResultForBadRequest();
        }

        try
        {
            var response =
                await Client.UpdateMediaGroup.ExecuteAsync(model.Id, model.NameGerman, model.NameEnglish,
                                                           model.DescriptionGerman, model.DescriptionEnglish,
                                                           model.EventDate.Value);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                model.ChangeDate = response.Data.UpdateMediaGroup.MediaGroup.ChangeDate;

                return CreateSimpleApiResult();
            }

            return CreateSimpleApiResultForError(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateSimpleApiResultForCommunicationError(ex);
        }
    }

    public async Task<IApiResult> DeleteMediaGroupAsync(IMediaGroupModel model, bool keepUploadItems)
    {
        try
        {
            var response = await Client.DeleteMediaGroup.ExecuteAsync(model.Id, keepUploadItems);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateSimpleApiResult();
            }

            return CreateSimpleApiResultForError(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateSimpleApiResultForCommunicationError(ex);
        }
    }
    #endregion
}