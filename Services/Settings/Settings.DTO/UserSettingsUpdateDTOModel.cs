using System.ComponentModel.DataAnnotations;

namespace Settings.DTO
{
    /// <summary>
    /// User-Setting Class for update operations from client to backend
    /// </summary>
    public class UserSettingsUpdateDTOModel
    {

        /// <summary>
        /// Should the question to keep the upload item be shown?
        /// </summary>
        [Required]
        public bool QuestionKeepUploadWhenDelete { get; set; }

        /// <summary>
        /// What should be the default when question would not be shown
        /// </summary>
        [Required]
        public bool DefaultKeepUploadWhenDelete { get; set; }
        
        /// <summary>
        /// Should videos automatically start playing when video player is shown
        /// </summary>
        [Required]
        public bool VideoAutoPlay { get; set; }

        /// <summary>
        /// The default volume vor video player all videos start playing with (100% - 0%)
        /// </summary>
        [Required]
        public int VideoVolume { get; set; }

        /// <summary>
        /// Should videos play in a loop
        /// </summary>
        [Required]
        public bool VideoLoop { get; set; }

        /// <summary>
        /// Should other videos of current album play after current video is fnished
        /// </summary>
        [Required]
        public bool VideoAutoPlayOtherVideos { get; set; }

        /// <summary>
        /// Wait time to play other video in of current album in seconds
        /// </summary>
        [Required]
        public int VideoTimeToPlayNextVideo { get; set; }

        /// <summary>
        /// Show forward button in Video-Player?
        /// </summary>
        [Required]
        public bool ShowButtonForward { get; set; }

        /// <summary>
        /// Show rewind button in Video-Player?
        /// </summary>
        [Required]
        public bool ShowButtonRewind { get; set; }

        /// <summary>
        /// Enable Zoom selection in Options-Menu
        /// </summary>
        [Required]
        public bool ShowZoomMenu { get; set; }

        /// <summary>
        /// Enable Play-Rate selecton in Options-Menu
        /// </summary>
        [Required]
        public bool ShowPlayRateMenu { get; set; }

        /// <summary>
        /// Show button for mirror video in Videoplayer?
        /// </summary>
        [Required]
        public bool ShowMirrorButton { get; set; }

        /// <summary>
        /// Show context menu for VideoPlayer?
        /// </summary>
        [Required]
        public bool ShowContextMenu { get; set; }

        /// <summary>
        /// Show quality selection in Options-Menu
        /// </summary>
        [Required]
        public bool ShowQualityMenu { get; set; }

        /// <summary>
        /// Show tooltip with play time on mouse cursor when over timeline
        /// </summary>
        [Required]
        public bool ShowTooltipForPlaytimeOnMouseCursor { get; set; }

        /// <summary>
        /// Show current play time as tooltip
        /// </summary>
        [Required]
        public bool ShowTooltipForCurrentPlaytime { get; set; }

        /// <summary>
        /// Show Zoom-Info in upper left corner of player
        /// </summary>
        [Required]
        public bool ShowZoomInfo { get; set; }

        /// <summary>
        /// Allow Zooming the video with mouse wheel
        /// </summary>
        [Required]
        public bool AllowZoomingWithMouseWheel { get; set; }

        /// <summary>
        /// Close gallery with pressing the Escape-Key
        /// </summary>
        [Required]
        public bool GalleryCloseEsc { get; set; }

        /// <summary>
        /// Close gallery with clicking on background
        /// </summary>
        [Required]
        public bool GalleryCloseDimmer { get; set; }

        /// <summary>
        /// Change current photo in gallery by using the mouse wheel
        /// </summary>
        [Required]
        public bool GalleryMouseWheelChangeSlide { get; set; }

        /// <summary>
        /// Show thumbnails from begin on
        /// </summary>
        [Required]
        public bool GalleryShowThumbnails { get; set; }

        /// <summary>
        /// Start gallery in full screen mode
        /// </summary>
        [Required]
        public bool GalleryShowFullScreen { get; set; }

        /// <summary>
        /// Time to transition from current picture to next picture in milliseconds
        /// </summary>
        [Required]
        public int GalleryTransitionDuration { get; set; }

        /// <summary>
        /// Name of the transition animation 
        /// </summary>
        [Required]
        public string GalleryTransitionType { get; set; }

        /// <summary>
        /// Use simple filter control in frontend
        /// </summary>
        [Required]
        public bool SimpleFilter { get; set; }
    }
}
