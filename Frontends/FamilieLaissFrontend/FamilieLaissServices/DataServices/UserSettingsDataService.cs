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
    public async Task<IApiResult> UpdateUserSettingsForUser(IUserSettingsModel? model)
    {
        if (model is null || string.IsNullOrWhiteSpace(model.Id))
        {
            return CreateSimpleApiResultForBadRequest();
        }

        try
        {
            var response = await Client.UpdateUserSetting.ExecuteAsync(model.Id, model.AllowZoomingWithMouseWheel,
                model.DefaultKeepUploadWhenDelete, model.GalleryCloseDimmer, model.GalleryCloseEsc,
                model.GalleryMouseWheelChangeSlide, model.GalleryShowFullScreen, model.GalleryShowThumbnails,
                model.GalleryTransitionDuration, model.GalleryTransitionType, model.QuestionKeepUploadWhenDelete,
                model.ShowButtonForward, model.ShowButtonRewind, model.ShowMirrorButton, model.ShowPlayRateMenu,
                model.ShowQualityMenu, model.ShowTooltipForCurrentPlaytime, model.ShowTooltipForPlaytimeOnMouseCursor,
                model.ShowZoomInfo, model.ShowZoomMenu, model.VideoAutoPlay, model.VideoAutoPlayOtherVideos,
                model.VideoLoop, model.VideoTimeToPlayNextVideo, model.VideoVolume, model.VideoTimeSeekForwardRewind);

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
