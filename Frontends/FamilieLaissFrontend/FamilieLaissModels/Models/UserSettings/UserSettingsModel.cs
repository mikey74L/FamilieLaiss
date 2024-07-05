using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.Models.UserSettings
{
    public partial class UserSettingsModel : IUserSettingsModel
    {
        public string Id { get; set; }

        public bool? VideoAutoPlay { get; set; }

        public int? VideoVolume { get; set; }

        public bool? VideoLoop { get; set; }

        public bool? VideoAutoPlayOtherVideos { get; set; }

        public int? VideoTimeToPlayNextVideo { get; set; }

        public bool? ShowButtonForward { get; set; }

        public bool? ShowButtonRewind { get; set; }

        public int? VideoTimeSeekForwardRewind { get; set; }

        public bool? ShowZoomMenu { get; set; }

        public bool? ShowPlayRateMenu { get; set; }

        public bool? ShowMirrorButton { get; set; }

        public bool? ShowQualityMenu { get; set; }

        public bool? ShowTooltipForPlaytimeOnMouseCursor { get; set; }

        public bool? ShowTooltipForCurrentPlaytime { get; set; }

        public bool? ShowZoomInfo { get; set; }

        public bool? AllowZoomingWithMouseWheel { get; set; }

        public bool? GalleryCloseEsc { get; set; }

        public bool? GalleryCloseDimmer { get; set; }

        public bool? GalleryMouseWheelChangeSlide { get; set; }

        public bool? GalleryShowThumbnails { get; set; }

        public bool? GalleryShowFullScreen { get; set; }

        public int? GalleryTransitionDuration { get; set; }

        public string? GalleryTransitionType { get; set; }

        public bool? QuestionKeepUploadWhenDelete { get; set; }

        public bool? DefaultKeepUploadWhenDelete { get; set; }

        public DateTimeOffset? CreateDate { get; set; }
        public DateTimeOffset? ChangeDate { get; set; }

        public UserSettingsModel()
        {
            Id = "";
            GalleryTransitionType = "";
        }

        public IUserSettingsModel Clone()
        {
            return new UserSettingsModel()
            {
                Id = this.Id,
                VideoAutoPlay = this.VideoAutoPlay,
                VideoVolume = this.VideoVolume,
                VideoLoop = this.VideoLoop,
                VideoAutoPlayOtherVideos = this.VideoAutoPlayOtherVideos,
                VideoTimeToPlayNextVideo = this.VideoTimeToPlayNextVideo,
                ShowButtonForward = this.ShowButtonForward,
                ShowButtonRewind = this.ShowButtonRewind,
                ShowZoomMenu = this.ShowZoomMenu,
                ShowPlayRateMenu = this.ShowPlayRateMenu,
                ShowMirrorButton = this.ShowMirrorButton,
                ShowQualityMenu = this.ShowQualityMenu,
                ShowTooltipForPlaytimeOnMouseCursor = this.ShowTooltipForPlaytimeOnMouseCursor,
                ShowTooltipForCurrentPlaytime = this.ShowTooltipForCurrentPlaytime,
                ShowZoomInfo = this.ShowZoomInfo,
                AllowZoomingWithMouseWheel = this.AllowZoomingWithMouseWheel,
                GalleryCloseEsc = this.GalleryCloseEsc,
                GalleryCloseDimmer = this.GalleryCloseDimmer,
                GalleryMouseWheelChangeSlide = this.GalleryMouseWheelChangeSlide,
                GalleryShowThumbnails = this.GalleryShowThumbnails,
                GalleryShowFullScreen = this.GalleryShowFullScreen,
                GalleryTransitionDuration = this.GalleryTransitionDuration,
                GalleryTransitionType = this.GalleryTransitionType,
                QuestionKeepUploadWhenDelete = this.QuestionKeepUploadWhenDelete,
                DefaultKeepUploadWhenDelete = this.DefaultKeepUploadWhenDelete,
                VideoTimeSeekForwardRewind = this.VideoTimeSeekForwardRewind,
                CreateDate = this.CreateDate,
                ChangeDate = this.ChangeDate
            };
        }

        public void TakeOverValues(IUserSettingsModel sourceModel)
        {
            throw new NotImplementedException("Take over values for this model is not implemented.");
        }
    }
}
