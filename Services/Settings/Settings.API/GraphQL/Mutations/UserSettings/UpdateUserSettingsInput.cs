namespace Settings.API.GraphQL.Mutations.UserSettings
{
    [GraphQLDescription("Input type for updating user settings")]
    public class UpdateUserSettingsInput
    {
        [GraphQLDescription("The ID for the user settings to update")]
        public string id { get; set; }

        #region Allgemeine Properties
        [GraphQLDescription("Should the simple filter control be used when filtering is available")]
        public bool SimpleFilter { get; set; }

        [GraphQLDescription("Should a question popup be shown to keep the upload item for deleted media items in the upload area.")]
        public bool QuestionKeepUploadWhenDelete { get; set; }

        [GraphQLDescription("Should the upload item for deleted media items be holded in the upload area as a default.")]
        public bool DefaultKeepUploadWhenDelete { get; set; }
        #endregion

        #region Properties für den Video-Player
        [GraphQLDescription("Should a video be automatically played when the video player is shown for the video?")]
        public bool VideoAutoPlay { get; set; }

        [GraphQLDescription("The default volume setting for all videos.")]
        public int VideoVolume { get; set; }

        [GraphQLDescription("Should videos be played in a loop.")]
        public bool VideoLoop { get; set; }

        [GraphQLDescription("Should other videos of current album play after current video is finished?")]
        public bool VideoAutoPlayOtherVideos { get; set; }

        [GraphQLDescription("Wait time to play other video of current album in seconds?")]
        public int VideoTimeToPlayNextVideo { get; set; }

        [GraphQLDescription("Show forward button in video player?")]
        public bool ShowButtonForward { get; set; }

        [GraphQLDescription("Show rewind button in video player?")]
        public bool ShowButtonRewind { get; set; }

        [GraphQLDescription("Enable zoom in options of video player?")]
        public bool ShowZoomMenu { get; set; }

        [GraphQLDescription("Show play rate selection in options of video player?")]
        public bool ShowPlayRateMenu { get; set; }

        [GraphQLDescription("Show button for mirror video in video player?")]
        public bool ShowMirrorButton { get; set; }

        [GraphQLDescription("Show context menu for video player?")]
        public bool ShowContextMenu { get; set; }

        [GraphQLDescription("Show quality selection in options of video player?")]
        public bool ShowQualityMenu { get; set; }

        [GraphQLDescription("Show tooltip with play time on mouse cursor when over timeline in video player?")]
        public bool ShowTooltipForPlaytimeOnMouseCursor { get; set; }

        [GraphQLDescription("Show current play time as tooltip in video player?")]
        public bool ShowTooltipForCurrentPlaytime { get; set; }

        [GraphQLDescription("Show zoom info in upper left corner of video player?")]
        public bool ShowZoomInfo { get; set; }

        [GraphQLDescription("Allow zooming the video in video player with mouse wheel?")]
        public bool AllowZoomingWithMouseWheel { get; set; }
        #endregion

        #region Properties für die Slideshow
        [GraphQLDescription("Close picture gallery by pressing the Escape-Key?")]
        public bool GalleryCloseEsc { get; set; }

        [GraphQLDescription("Close picture gallery by clicking on background?")]
        public bool GalleryCloseDimmer { get; set; }

        [GraphQLDescription("Change current photo in picture gallery by using the mouse wheel?")]
        public bool GalleryMouseWheelChangeSlide { get; set; }

        [GraphQLDescription("Show thumbnails in picture gallery?")]
        public bool GalleryShowThumbnails { get; set; }

        [GraphQLDescription("Start picture gallery in full screen mode?")]
        public bool GalleryShowFullScreen { get; set; }

        [GraphQLDescription("Time to transition from current picture to next picture in milliseconds?")]
        public int GalleryTransitionDuration { get; set; }

        [GraphQLDescription("Name of the transition animation?")]
        public string GalleryTransitionType { get; set; }
        #endregion
    }
}
