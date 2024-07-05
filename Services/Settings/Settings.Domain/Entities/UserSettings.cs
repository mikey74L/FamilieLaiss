using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using HotChocolate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Domain.Entities
{
    /// <summary>
    /// Entity for user settings
    /// </summary>
    [GraphQLDescription("User-Settings")]
    public class UserSettings : EntityModify<string>
    {
        #region Allgemeine Properties
        /// <summary>
        /// Use simple filter control in frontend
        /// </summary>
        [GraphQLDescription("Should the simple filter control be used when filtering is available")]
        [GraphQLNonNullType]
        public bool SimpleFilter { get; private set; }

        /// <summary>
        /// Should the question to keep the upload item be shown?
        /// </summary>
        [GraphQLDescription("Should a question popup be shown to keep the upload item for deleted media items in the upload area.")]
        [GraphQLNonNullType]
        public bool QuestionKeepUploadWhenDelete { get; private set; }

        /// <summary>
        /// What should be the default when question would not be shown
        /// </summary>
        [GraphQLDescription("Should the upload item for deleted media items be holded in the upload area as a default.")]
        [GraphQLNonNullType]
        public bool DefaultKeepUploadWhenDelete { get; private set; }
        #endregion

        #region Properties für den Video-Player
        /// <summary>
        /// Should videos automatically start playing when video player is shown
        /// </summary>
        [GraphQLDescription("Should a video be automatically played when the video player is shown for the video?")]
        [GraphQLNonNullType]
        public bool VideoAutoPlay { get; private set; }

        /// <summary>
        /// The default volume vor video player all videos start playing with (100% - 0%)
        /// </summary>
        [GraphQLDescription("The default volume setting for all videos.")]
        [GraphQLNonNullType]
        public int VideoVolume { get; private set; }

        /// <summary>
        /// Should videos play in a loop
        /// </summary>
        [GraphQLDescription("Should videos be played in a loop.")]
        [GraphQLNonNullType]
        public bool VideoLoop { get; private set; }

        /// <summary>
        /// Should other videos of current album play after current video is finished
        /// </summary>
        [GraphQLDescription("Should other videos of current album play after current video is finished?")]
        [GraphQLNonNullType]
        public bool VideoAutoPlayOtherVideos { get; private set; }

        /// <summary>
        /// Wait time to play other video of current album in seconds
        /// </summary>
        [GraphQLDescription("Wait time to play other video of current album in seconds?")]
        [GraphQLNonNullType]
        public int VideoTimeToPlayNextVideo { get; private set; }

        /// <summary>
        /// Show forward button in Video-Player?
        /// </summary>
        [GraphQLDescription("Show forward button in video player?")]
        [GraphQLNonNullType]
        public bool ShowButtonForward { get; private set; }

        /// <summary>
        /// Show rewind button in Video-Player?
        /// </summary>
        [GraphQLDescription("Show rewind button in video player?")]
        [GraphQLNonNullType]
        public bool ShowButtonRewind { get; private set; }

        /// <summary>
        /// Enable Zoom selection in Options-Menu
        /// </summary>
        [GraphQLDescription("Enable zoom in options of video player?")]
        [GraphQLNonNullType]
        public bool ShowZoomMenu { get; private set; }

        /// <summary>
        /// Enable Play-Rate selecton in Options-Menu
        /// </summary>
        [GraphQLDescription("Show play rate selection in options of video player?")]
        [GraphQLNonNullType]
        public bool ShowPlayRateMenu { get; private set; }

        /// <summary>
        /// Show button for mirror video in Videoplayer?
        /// </summary>
        [GraphQLDescription("Show button for mirror video in video player?")]
        [GraphQLNonNullType]
        public bool ShowMirrorButton { get; private set; }

        /// <summary>
        /// Show context menu for VideoPlayer?
        /// </summary>
        [GraphQLDescription("Show context menu for video player?")]
        [GraphQLNonNullType]
        public bool ShowContextMenu { get; private set; }

        /// <summary>
        /// Show quality selection in Options-Menu
        /// </summary>
        [GraphQLDescription("Show quality selection in options of video player?")]
        [GraphQLNonNullType]
        public bool ShowQualityMenu { get; private set; }

        /// <summary>
        /// Show tooltip with play time on mouse cursor when over timeline
        /// </summary>
        [GraphQLDescription("Show tooltip with play time on mouse cursor when over timeline in video player?")]
        [GraphQLNonNullType]
        public bool ShowTooltipForPlaytimeOnMouseCursor { get; private set; }

        /// <summary>
        /// Show current play time as tooltip
        /// </summary>
        [GraphQLDescription("Show current play time as tooltip in video player?")]
        [GraphQLNonNullType]
        public bool ShowTooltipForCurrentPlaytime { get; private set; }

        /// <summary>
        /// Show Zoom-Info in upper left corner of player
        /// </summary>
        [GraphQLDescription("Show zoom info in upper left corner of video player?")]
        [GraphQLNonNullType]
        public bool ShowZoomInfo { get; private set; }

        /// <summary>
        /// Allow Zooming the video with mouse wheel
        /// </summary>
        [GraphQLDescription("Allow zooming the video in video player with mouse wheel?")]
        [GraphQLNonNullType]
        public bool AllowZoomingWithMouseWheel { get; private set; }
        #endregion

        #region Properties für die Slideshow
        /// <summary>
        /// Close gallery with pressing the Escape-Key
        /// </summary>
        [GraphQLDescription("Close picture gallery by pressing the Escape-Key?")]
        [GraphQLNonNullType]
        public bool GalleryCloseEsc { get; private set; }

        /// <summary>
        /// Close gallery with clicking on background
        /// </summary>
        [GraphQLDescription("Close picture gallery by clicking on background?")]
        [GraphQLNonNullType]
        public bool GalleryCloseDimmer { get; private set; }

        /// <summary>
        /// Change current photo in gallery by using the mouse wheel
        /// </summary>
        [GraphQLDescription("Change current photo in picture gallery by using the mouse wheel?")]
        [GraphQLNonNullType]
        public bool GalleryMouseWheelChangeSlide { get; private set; }

        /// <summary>
        /// Show thumbnails from begin on
        /// </summary>
        [GraphQLDescription("Show thumbnails in picture gallery?")]
        [GraphQLNonNullType]
        public bool GalleryShowThumbnails { get; private set; }

        /// <summary>
        /// Start gallery in full screen mode
        /// </summary>
        [GraphQLDescription("Start picture gallery in full screen mode?")]
        [GraphQLNonNullType]
        public bool GalleryShowFullScreen { get; private set; }

        /// <summary>
        /// Time to transition from current picture to next picture in milliseconds
        /// </summary>
        [GraphQLDescription("Time to transition from current picture to next picture in milliseconds?")]
        [GraphQLNonNullType]
        public int GalleryTransitionDuration { get; private set; }

        /// <summary>
        /// Name of the transition animation 
        /// </summary>
        [GraphQLDescription("Name of the transition animation?")]
        [GraphQLNonNullType]
        public string GalleryTransitionType { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor without parameters would be used by EF-Core
        /// </summary>
        private UserSettings()
        {

        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="userName">Username of user to which this setting belongs to</param>
        public UserSettings(string userName) : base()
        {
            //Übernehmen des Benutzernamen
            Id = userName;

            //Bei einem neuen User-Setting werden bestimmte Default-Werte gesetzt
            GalleryCloseDimmer = false;
            GalleryCloseEsc = false;
            GalleryMouseWheelChangeSlide = false;
            GalleryShowFullScreen = true;
            GalleryShowThumbnails = true;
            GalleryTransitionDuration = 600;
            GalleryTransitionType = "lg-fade";
            SimpleFilter = true;
            VideoAutoPlay = true;
            VideoAutoPlayOtherVideos = true;
            VideoLoop = false;
            VideoTimeToPlayNextVideo = 5;
            VideoVolume = 100;
            QuestionKeepUploadWhenDelete = true;
            DefaultKeepUploadWhenDelete = true;
            ShowButtonForward = true;
            ShowButtonRewind = true;
            ShowZoomMenu = true;
            ShowPlayRateMenu = true;
            ShowMirrorButton = true;
            ShowContextMenu = true;
            ShowQualityMenu = true;
            ShowTooltipForPlaytimeOnMouseCursor = true;
            ShowTooltipForCurrentPlaytime = true;
            ShowZoomInfo = true;
            AllowZoomingWithMouseWheel = true;
        }
        #endregion

        #region Domain Methods
        public void UpdateSettings(bool videoAutoPlay, int videoVolume, bool videoLoop,
                bool videoAutoPlayOtherVideos, int videoTimeToPlayNextVideo,
                bool showButtonForward, bool showButtonRewind, bool showZoomMenu, bool showPlayRateMenu, bool showMirrorButton, bool showContextMenu,
                bool showQualityMenu, bool showTooltipForPlaytimeOnMouseCursor, bool showTooltipForCurrentPlaytime, bool showZoomInfo,
                bool allowZoomingWithMouseWheel,
                bool galleryCloseEsc, bool galleryCloseDimmer,
                bool galleryMouseWheelChangeSlide, bool galleryShowThumbnails, bool galleryShowFullScreen, int galleryTransitionDuration,
                string galleryTransitionType, bool simpleFilter, bool questionKeepUploadWhenDelete, bool defaultKeepUploadWhenDelete)
        {
            //Überprüfen des Volumes des Volumes
            if (videoVolume < 0 || videoVolume > 100)
            {
                throw new DomainException("Volume have to be between 0 and 100");
            }

            //Überprüfen der Abspielzeit für nächstes Video
            if (videoTimeToPlayNextVideo < 2 || videoTimeToPlayNextVideo > 10)
            {
                throw new DomainException("Time to play next video must be between 2 and 10");
            }

            //Überprüfen der Duration Time
            if (galleryTransitionDuration < 500 || galleryTransitionDuration > 900)
            {
                throw new DomainException("Gallery transition duration must be between 500 and 900");
            }

            //Überprüfen des Transition-Types
            if (galleryTransitionType != "lg-fade" && galleryTransitionType != "lg-zoom-in" &&
                galleryTransitionType != "lg-zoom-in-big" && galleryTransitionType != "lg-zoom-out" &&
                galleryTransitionType != "lg-zoom-out-big" && galleryTransitionType != "lg-zoom-out-in" &&
                galleryTransitionType != "lg-zoom-in-out" && galleryTransitionType != "lg-soft-zoom" &&
                galleryTransitionType != "lg-scale-up" && galleryTransitionType != "lg-slide-circular" &&
                galleryTransitionType != "lg-slide-circular-vertical" && galleryTransitionType != "lg-slide-vertical" &&
                galleryTransitionType != "lg-slide-vertical-growth" && galleryTransitionType != "lg-slide-skew-only" &&
                galleryTransitionType != "lg-slide-skew-only-rev" && galleryTransitionType != "lg-slide-skew-only-y" &&
                galleryTransitionType != "lg-slide-skew-only-y-rev" && galleryTransitionType != "" &&
                galleryTransitionType != "lg-slide-skew" && galleryTransitionType != "lg-slide-skew-rev" &&
                galleryTransitionType != "lg-slide-skew-cross" && galleryTransitionType != "lg-slide-skew-cross-rev" &&
                galleryTransitionType != "lg-slide-skew-ver" &&
                galleryTransitionType != "lg-slide-skew-ver-cross" && galleryTransitionType != "lg-slide-skew-ver-rev" &&
                galleryTransitionType != "lg-slide-skew-ver-cross-rev" && galleryTransitionType != "lg-lollipop-rev" &&
                galleryTransitionType != "lg-lollipop" && galleryTransitionType != "lg-rotate" &&
                galleryTransitionType != "lg-rotate-rev" && galleryTransitionType != "lg-tube")
            {
                throw new DomainException("Wrong transition type for gallery");
            }

            //Übernehmen der neuen Daten
            VideoAutoPlay = videoAutoPlay;
            VideoVolume = videoVolume;
            VideoLoop = videoLoop;
            VideoAutoPlayOtherVideos = videoAutoPlayOtherVideos;
            VideoTimeToPlayNextVideo = videoTimeToPlayNextVideo;
            GalleryCloseEsc = galleryCloseEsc;
            GalleryCloseDimmer = galleryCloseDimmer;
            GalleryMouseWheelChangeSlide = galleryMouseWheelChangeSlide;
            GalleryShowThumbnails = galleryShowThumbnails;
            GalleryShowFullScreen = galleryShowFullScreen;
            GalleryTransitionDuration = galleryTransitionDuration;
            GalleryTransitionType = galleryTransitionType;
            SimpleFilter = simpleFilter;
            QuestionKeepUploadWhenDelete = questionKeepUploadWhenDelete;
            DefaultKeepUploadWhenDelete = defaultKeepUploadWhenDelete;
            ShowButtonForward = showButtonForward;
            ShowButtonRewind = showButtonRewind;
            ShowZoomMenu = showZoomMenu;
            ShowPlayRateMenu = showPlayRateMenu;
            ShowMirrorButton = showMirrorButton;
            ShowContextMenu = showContextMenu;
            ShowQualityMenu = showQualityMenu;
            ShowTooltipForPlaytimeOnMouseCursor = showTooltipForPlaytimeOnMouseCursor;
            ShowTooltipForCurrentPlaytime = showTooltipForCurrentPlaytime;
            ShowZoomInfo = showZoomInfo;
            AllowZoomingWithMouseWheel = allowZoomingWithMouseWheel;
        }
        #endregion

        #region Called from Change-Tracker
        public override Task EntityModifiedAsync()
        {
            return Task.CompletedTask;
        }

        public override Task EntityAddedAsync()
        {
            return Task.CompletedTask;
        }

        public override Task EntityDeletedAsync()
        {
            return Task.CompletedTask;
        }
        #endregion
    }
}
