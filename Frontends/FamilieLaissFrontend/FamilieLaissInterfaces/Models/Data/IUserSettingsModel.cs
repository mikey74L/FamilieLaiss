namespace FamilieLaissInterfaces.Models.Data
{
    public interface IUserSettingsModel : IBaseModel<IUserSettingsModel>
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
    }
}
