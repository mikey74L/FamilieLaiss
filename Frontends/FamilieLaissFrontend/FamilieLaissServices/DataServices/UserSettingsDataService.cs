using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissMappingExtensions.UserSetting;
using StrawberryShake;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FamilieLaissServices.DataServices;

public class UserSettingsDataService(IFamilieLaissClient familieLaissClient) : BaseDataService(familieLaissClient), IUserSettingsDataService
{
    #region Query
    public async Task<IApiResult<IUserSettingsModel>> GetUserSettingsForUserAsync(string id)
    {
        try
        {
            var response = await Client.GetUserSettingForUser.ExecuteAsync(id);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                if (response.Data.UserSettings.Count > 0)
                {
                    return CreateApiResult(response.Data.UserSettings.First().Map());
                }
                else
                {
                    return await CreateUserSettingsForUser(id);
                }
            }

            return CreateApiResultForError<IUserSettingsModel>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<IUserSettingsModel>(ex);
        }
    }
    #endregion

    #region CRUD
    private async Task<IApiResult<IUserSettingsModel>> CreateUserSettingsForUser(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return CreateApiResultForBadRequest<IUserSettingsModel>();
        }

        try
        {
            var response =
                await Client.AddUserSetting.ExecuteAsync(id);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.AddUserSetting.UserSetting.Map());
            }

            return CreateApiResultForError<IUserSettingsModel>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<IUserSettingsModel>(ex);
        }
    }

    public async Task<IApiResult> UpdateUserSettingsForUser(IUserSettingsModel? model)
    {
        if (model is null || string.IsNullOrWhiteSpace(model.Id))
        {
            return CreateSimpleApiResultForBadRequest();
        }
        if (model.AllowZoomingWithMouseWheel is null || model.AllowZoomingWithMouseWheel is null ||
            model.DefaultKeepUploadWhenDelete is null || model.GalleryCloseDimmer is null || model.GalleryCloseEsc is null ||
            model.GalleryMouseWheelChangeSlide is null || model.GalleryShowFullScreen is null || model.GalleryShowThumbnails is null ||
            model.GalleryTransitionDuration is null || model.GalleryTransitionType is null || model.QuestionKeepUploadWhenDelete is null ||
            model.ShowButtonForward is null || model.ShowButtonRewind is null || model.ShowMirrorButton is null ||
            model.ShowPlayRateMenu is null || model.ShowQualityMenu is null || model.ShowTooltipForCurrentPlaytime is null ||
            model.ShowTooltipForPlaytimeOnMouseCursor is null || model.ShowZoomInfo is null || model.ShowZoomMenu is null ||
            model.VideoAutoPlay is null || model.VideoAutoPlayOtherVideos is null || model.VideoLoop is null ||
            model.VideoTimeToPlayNextVideo is null || model.VideoVolume is null ||
            model.VideoTimeSeekForwardRewind is null || string.IsNullOrWhiteSpace(model.GalleryTransitionType))
        {
            return CreateSimpleApiResultForBadRequest();
        }

        try
        {
            var response = await Client.UpdateUserSetting.ExecuteAsync(model.Id, model.AllowZoomingWithMouseWheel.Value,
                model.DefaultKeepUploadWhenDelete.Value, model.GalleryCloseDimmer.Value, model.GalleryCloseEsc.Value,
                model.GalleryMouseWheelChangeSlide.Value, model.GalleryShowFullScreen.Value, model.GalleryShowThumbnails.Value,
                model.GalleryTransitionDuration.Value, model.GalleryTransitionType, model.QuestionKeepUploadWhenDelete.Value,
                model.ShowButtonForward.Value, model.ShowButtonRewind.Value, model.ShowMirrorButton.Value, model.ShowPlayRateMenu.Value,
                model.ShowQualityMenu.Value, model.ShowTooltipForCurrentPlaytime.Value, model.ShowTooltipForPlaytimeOnMouseCursor.Value,
                model.ShowZoomInfo.Value, model.ShowZoomMenu.Value, model.VideoAutoPlay.Value, model.VideoAutoPlayOtherVideos.Value,
                model.VideoLoop.Value, model.VideoTimeToPlayNextVideo.Value, model.VideoVolume.Value, model.VideoTimeSeekForwardRewind.Value);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                model.ChangeDate = response.Data.UpdateUserSetting.UserSetting.ChangeDate;

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
