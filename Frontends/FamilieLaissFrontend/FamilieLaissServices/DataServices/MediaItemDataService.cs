using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissMappingExtensions.MediaItem;
using StrawberryShake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilieLaissServices.DataServices;

public class MediaItemDataService(IFamilieLaissClient familieLaissClient)
    : BaseDataService(familieLaissClient), IMediaItemDataService
{
    #region Validation
    public async Task<IApiResult<bool>> EnglishMediaItemNameExistsAsync(long id, long mediaGroupId, string? englishName)
    {
        try
        {
            var response = await Client.EnglishMediaItemNameExists.ExecuteAsync(englishName ?? "", mediaGroupId, id);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.EnglishMediaItemNameExists);
            }

            return CreateApiResultForError<bool>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<bool>(ex);
        }
    }

    public async Task<IApiResult<bool>> GermanMediaItemNameExistsAsync(long id, long mediaGroupId, string? germanName)
    {
        try
        {
            var response = await Client.GermanMediaItemNameExists.ExecuteAsync(germanName ?? "", mediaGroupId, id);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.GermanMediaItemNameExists);
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
    public async Task<IApiResult<IMediaItemModel>> GetMediaItemAsync(long mediaItemId)
    {
        try
        {
            var response = await Client.GetMediaItem.ExecuteAsync(mediaItemId);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.MediaItems.First().Map());
            }

            return CreateApiResultForError<IMediaItemModel>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<IMediaItemModel>(ex);
        }
    }

    public async Task<IApiResult<IEnumerable<IMediaItemModel>>> GetMediaItemsForGroupAsync(IMediaGroupModel mediaGroup)
    {
        try
        {
            var response = await Client.GetMediaItemsForMediaGroup.ExecuteAsync(mediaGroup.Id);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.MediaItems.Map());
            }

            return CreateApiResultForError<IEnumerable<IMediaItemModel>>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<IEnumerable<IMediaItemModel>>(ex);
        }
    }
    #endregion

    #region CRUD
    public async Task<IApiResult> AddMediaItemAsync(IMediaItemModel model, List<long> assignedCategoryValues)
    {
        if (model.MediaGroupId is null)
        {
            return CreateSimpleApiResultForBadRequest();
        }
        if (model.MediaType is null)
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model.MediaType == EnumMediaType.Video && string.IsNullOrWhiteSpace(model.NameGerman))
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model.MediaType == EnumMediaType.Video && string.IsNullOrWhiteSpace(model.NameEnglish))
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model.MediaType == EnumMediaType.Video && string.IsNullOrWhiteSpace(model.DescriptionGerman))
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model.MediaType == EnumMediaType.Video && string.IsNullOrWhiteSpace(model.DescriptionEnglish))
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model is { MediaType: EnumMediaType.Picture, UploadPicture: null })
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model is { MediaType: EnumMediaType.Video, UploadVideo: null })
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model.OnlyFamily is null)
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model.MediaType == EnumMediaType.Picture)
        {
            model.NameGerman = "Platzhalter";
            model.NameEnglish = "Placeholder";
        }

        try
        {
            var response =
                await Client.AddMediaItem.ExecuteAsync(model.MediaGroupId.Value, model.MediaType.Value,
                                                       model.NameGerman!, model.NameEnglish!,
                                                       model.DescriptionGerman, model.DescriptionEnglish,
                                                       model.OnlyFamily.Value,
                                                       model.MediaType == EnumMediaType.Picture ? model.UploadPicture!.Id : model.UploadVideo!.Id,
                                                       assignedCategoryValues);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                model.Id = response.Data.AddMediaItem.MediaItem.Id;
                model.CreateDate = response.Data.AddMediaItem.MediaItem.CreateDate;

                return CreateSimpleApiResult();
            }

            return CreateSimpleApiResultForError(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateSimpleApiResultForCommunicationError(ex);
        }
    }

    public async Task<IApiResult> UpdateMediaItemAsync(IMediaItemModel model, List<long> assignedCategoryValues)
    {
        if (model.MediaType == EnumMediaType.Video && string.IsNullOrWhiteSpace(model.NameGerman))
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model.MediaType == EnumMediaType.Video && string.IsNullOrWhiteSpace(model.NameEnglish))
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model.MediaType == EnumMediaType.Video && string.IsNullOrWhiteSpace(model.DescriptionGerman))
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model.MediaType == EnumMediaType.Video && string.IsNullOrWhiteSpace(model.DescriptionEnglish))
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model is { MediaType: EnumMediaType.Picture, UploadPicture: null })
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model is { MediaType: EnumMediaType.Video, UploadVideo: null })
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model.OnlyFamily is null)
        {
            return CreateSimpleApiResultForBadRequest();
        }

        try
        {
            var response =
                await Client.UpdateMediaItem.ExecuteAsync(model.Id, model.NameGerman!, model.NameEnglish!,
                                                          model.DescriptionGerman, model.DescriptionEnglish,
                                                          model.OnlyFamily.Value, assignedCategoryValues);

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

    public async Task<IApiResult> DeleteMediaItemAsync(IMediaItemModel model, bool keepUploadItem)
    {
        if (model.MediaGroupId is null)
        {
            return CreateSimpleApiResultForBadRequest();
        }

        try
        {
            var response = await Client.DeleteMediaItem.ExecuteAsync(model.MediaGroupId.Value, model.Id, keepUploadItem);

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

