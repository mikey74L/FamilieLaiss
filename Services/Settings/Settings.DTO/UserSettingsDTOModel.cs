namespace Settings.DTO
{
    public class UserSettingsDTOModel
    {
        /// <summary>
        /// ID of the item
        /// </summary>
        public string ID { get; set; }

        #region Allgemeine Properties
        /// <summary>
        /// Use simple filter control in frontend
        /// </summary>
        public bool SimpleFilter { get; set; }

        /// <summary>
        /// Should the question to keep the upload item be shown?
        /// </summary>
        public bool QuestionKeepUploadWhenDelete { get; set; }

        /// <summary>
        /// What should be the default when question would not be shown
        /// </summary>
        public bool DefaultKeepUploadWhenDelete { get; set; }
        #endregion

        #region Properties für den Video-Player
        /// <summary>
        /// Should videos automatically start playing when video player is shown
        /// </summary>
        public bool VideoAutoPlay { get; set; }

        /// <summary>
        /// The default volume vor video player all videos start playing with (100% - 0%)
        /// </summary>
        public int VideoVolume { get; set; }

        /// <summary>
        /// Should videos play in a loop
        /// </summary>
        public bool VideoLoop { get; set; }

        /// <summary>
        /// Should other videos of current album play after current video is fnished
        /// </summary>
        public bool VideoAutoPlayOtherVideos { get; set; }

        /// <summary>
        /// Wait time to play other video in of current album in seconds
        /// </summary>
        public int VideoTimeToPlayNextVideo { get; set; }

        /// <summary>
        /// Show forward button in Video-Player?
        /// </summary>
        public bool ShowButtonForward { get; set; }

        /// <summary>
        /// Show rewind button in Video-Player?
        /// </summary>
        public bool ShowButtonRewind { get; set; }

        /// <summary>
        /// Enable Zoom selection in Options-Menu
        /// </summary>
        public bool ShowZoomMenu { get; set; }

        /// <summary>
        /// Enable Play-Rate selecton in Options-Menu
        /// </summary>
        public bool ShowPlayRateMenu { get; set; }

        /// <summary>
        /// Show button for mirror video in Videoplayer?
        /// </summary>
        public bool ShowMirrorButton { get; set; }

        /// <summary>
        /// Show context menu for VideoPlayer?
        /// </summary>
        public bool ShowContextMenu { get; set; }

        /// <summary>
        /// Show quality selection in Options-Menu
        /// </summary>
        public bool ShowQualityMenu { get; set; }

        /// <summary>
        /// Show tooltip with play time on mouse cursor when over timeline
        /// </summary>
        public bool ShowTooltipForPlaytimeOnMouseCursor { get; set; }

        /// <summary>
        /// Show current play time as tooltip
        /// </summary>
        public bool ShowTooltipForCurrentPlaytime { get; set; }

        /// <summary>
        /// Show Zoom-Info in upper left corner of player
        /// </summary>
        public bool ShowZoomInfo { get; set; }

        /// <summary>
        /// Allow Zooming the video with mouse wheel
        /// </summary>
        public bool AllowZoomingWithMouseWheel { get; set; }
        #endregion

        #region Properties für die Slideshow
        /// <summary>
        /// Close gallery with pressing the Escape-Key
        /// </summary>
        public bool GalleryCloseEsc { get; set; }

        /// <summary>
        /// Close gallery with clicking on background
        /// </summary>
        public bool GalleryCloseDimmer { get; set; }

        /// <summary>
        /// Change current photo in gallery by using the mouse wheel
        /// </summary>
        public bool GalleryMouseWheelChangeSlide { get; set; }

        /// <summary>
        /// Show thumbnails from begin on
        /// </summary>
        public bool GalleryShowThumbnails { get; set; }

        /// <summary>
        /// Start gallery in full screen mode
        /// </summary>
        public bool GalleryShowFullScreen { get; set; }

        /// <summary>
        /// Time to transition from current picture to next picture in milliseconds
        /// </summary>
        public int GalleryTransitionDuration { get; set; }

        /// <summary>
        /// Name of the transition animation 
        /// </summary>
        public string GalleryTransitionType { get; set; }
        #endregion
    }
}
