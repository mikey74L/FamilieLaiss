using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissModels.Models.UserSettings;

namespace FamilieLaissMappingExtensions.UserSetting;

public static class UserSettingMappingExtension
{
    public static IUserSettingsModel Map(this IFrUserSettingFull sourceItem)
    {
        var newItem = new UserSettingsModel()
        {
            Id = sourceItem.Id,
            AllowZoomingWithMouseWheel = sourceItem.AllowZoomingWithMouseWheel,
            DefaultKeepUploadWhenDelete = sourceItem.DefaultKeepUploadWhenDelete,
            GalleryCloseDimmer = sourceItem.GalleryCloseDimmer,
            GalleryCloseEsc = sourceItem.GalleryCloseEsc,
            GalleryMouseWheelChangeSlide = sourceItem.GalleryMouseWheelChangeSlide,
            GalleryShowFullScreen = sourceItem.GalleryShowFullScreen,
            GalleryShowThumbnails = sourceItem.GalleryShowThumbnails,
            GalleryTransitionDuration = sourceItem.GalleryTransitionDuration,
            GalleryTransitionType = sourceItem.GalleryTransitionType,
            ShowButtonForward = sourceItem.ShowButtonForward,
            ShowButtonRewind = sourceItem.ShowButtonRewind,
            QuestionKeepUploadWhenDelete = sourceItem.QuestionKeepUploadWhenDelete,
            ShowMirrorButton = sourceItem.ShowMirrorButton,
            ShowPlayRateMenu = sourceItem.ShowPlayRateMenu,
            ShowQualityMenu = sourceItem.ShowQualityMenu,
            ShowTooltipForCurrentPlaytime = sourceItem.ShowTooltipForCurrentPlaytime,
            ShowTooltipForPlaytimeOnMouseCursor = sourceItem.ShowTooltipForPlaytimeOnMouseCursor,
            ShowZoomInfo = sourceItem.ShowZoomInfo,
            ShowZoomMenu = sourceItem.ShowZoomMenu,
            VideoAutoPlay = sourceItem.VideoAutoPlay,
            VideoAutoPlayOtherVideos = sourceItem.VideoAutoPlayOtherVideos,
            VideoLoop = sourceItem.VideoLoop,
            VideoTimeSeekForwardRewind = sourceItem.VideoTimeSeekForwardRewind,
            VideoTimeToPlayNextVideo = sourceItem.VideoTimeToPlayNextVideo,
            VideoVolume = sourceItem.VideoVolume,
            CreateDate = sourceItem.CreateDate,
            ChangeDate = sourceItem.ChangeDate
        };

        return newItem;
    }
}
