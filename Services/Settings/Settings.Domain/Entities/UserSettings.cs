using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using Settings.Domain.DomainEvents;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Settings.Domain.Entities;

/// <summary>
/// Entity for user settings
/// </summary>
[GraphQLDescription("User setting")]
public class UserSetting : EntityModify<string>
{
    #region Properties
    #region Media Item
    /// <summary>
    /// Should the question to keep the upload item be shown?
    /// </summary>
    [GraphQLDescription("Should the question to keep the upload item be shown?")]
    [Required]
    public bool QuestionKeepUploadWhenDelete { get; private set; }

    /// <summary>
    /// What should be the default when question would not be shown
    /// </summary>
    [GraphQLDescription("What should be the default when question would not be shown")]
    [Required]
    public bool DefaultKeepUploadWhenDelete { get; private set; }
    #endregion

    #region Video-Player
    /// <summary>
    /// Should videos automatically start playing when video player is shown
    /// </summary>
    [GraphQLDescription("Should videos automatically start playing when video player is shown")]
    [Required]
    public bool VideoAutoPlay { get; private set; }

    /// <summary>
    /// The default volume vor video player all videos start playing with 
    /// </summary>
    [GraphQLDescription("The default volume vor video player all videos start playing with")]
    [Required]
    public int VideoVolume { get; private set; }

    /// <summary>
    /// Should videos play in a loop
    /// </summary>
    [GraphQLDescription("Should videos play in a loop")]
    [Required]
    public bool VideoLoop { get; private set; }

    /// <summary>
    /// Should other videos of current album play after current video is fnished
    /// </summary>
    [GraphQLDescription("Should other videos of current album play after current video is fnished")]
    [Required]
    public bool VideoAutoPlayOtherVideos { get; private set; }

    /// <summary>
    /// Wait time to play other video in of current album in seconds
    /// </summary>
    [GraphQLDescription("Wait time to play other video in of current album in seconds")]
    [Required]
    public int VideoTimeToPlayNextVideo { get; private set; }

    /// <summary>
    /// Show zoom info in video player
    /// </summary>
    [GraphQLDescription("Show zoom info in video player")]
    [Required]
    public bool ShowZoomInfo { get; private set; }

    /// <summary>
    /// Allow zoom with mouse wheel in video player
    /// </summary>
    [GraphQLDescription("Allow zoom with mouse wheel in video player")]
    [Required]
    public bool AllowZoomingWithMouseWheel { get; private set; }

    /// <summary>
    /// Show tooltip for playtime in video player when mouse over
    /// </summary>
    [GraphQLDescription("Show tooltip for playtime in video player when mouse over")]
    [Required]
    public bool ShowTooltipForPlaytimeOnMouseCursor { get; private set; }

    /// <summary>
    /// Show tooltip for current playtime in video player
    /// </summary>
    [GraphQLDescription("Show tooltip for current playtime in video player")]
    [Required]
    public bool ShowTooltipForCurrentPlaytime { get; private set; }

    /// <summary>
    /// Should the seek forward button be shown in the player
    /// </summary>
    [GraphQLDescription("Should the seek forward button be shown in the player")]
    [Required]
    public bool ShowButtonForward { get; private set; }

    /// <summary>
    /// Should the seek backward button be shown in the player
    /// </summary>
    [GraphQLDescription("Should the seek backward button be shown in the player")]
    [Required]
    public bool ShowButtonRewind { get; private set; }

    /// <summary>
    /// Should the player show the zoom menu
    /// </summary>
    [GraphQLDescription("Should the player show the zoom menu")]
    [Required]
    public bool ShowZoomMenu { get; private set; }

    /// <summary>
    /// Show play rate menu in video player
    /// </summary>
    [GraphQLDescription("Show play rate menu in video player")]
    [Required]
    public bool ShowPlayRateMenu { get; private set; }

    /// <summary>
    /// Show mirror button in video player
    /// </summary>
    [GraphQLDescription("Show mirror button in video player")]
    [Required]
    public bool ShowMirrorButton { get; private set; }

    /// <summary>
    /// Show quality menu in video player
    /// </summary>
    [GraphQLDescription("Show quality menu in video player")]
    [Required]
    public bool ShowQualityMenu { get; private set; }

    /// <summary>
    /// Time in seconds the seek forward / rewind button will jump ahead / back in the video
    /// </summary>
    [GraphQLDescription("Time in seconds the seek forward / rewind button will jump ahead / back in the video")]
    [Required]
    public int VideoTimeSeekForwardRewind { get; private set; }
    #endregion

    #region Slide-Show
    /// <summary>
    /// Close gallery with pressing the Escape-Key
    /// </summary>
    [GraphQLDescription("Close gallery with pressing the Escape-Key")]
    [Required]
    public bool GalleryCloseEsc { get; private set; }

    /// <summary>
    /// Close gallery with clicking on background
    /// </summary>
    [GraphQLDescription("Close gallery with clicking on background")]
    [Required]
    public bool GalleryCloseDimmer { get; private set; }

    /// <summary>
    /// Change current photo in gallery by using the mouse wheel
    /// </summary>
    [GraphQLDescription("Change current photo in gallery by using the mouse wheel")]
    [Required]
    public bool GalleryMouseWheelChangeSlide { get; private set; }

    /// <summary>
    /// Show thumbnails from begin on
    /// </summary>
    [GraphQLDescription("Show thumbnails from begin on")]
    [Required]
    public bool GalleryShowThumbnails { get; private set; }

    /// <summary>
    /// Start gallery in full screen mode
    /// </summary>
    [GraphQLDescription("Start gallery in full screen mode")]
    [Required]
    public bool GalleryShowFullScreen { get; private set; }

    /// <summary>
    /// Time to transition from current picture to next picture in milliseconds
    /// </summary>
    [GraphQLDescription("Time to transition from current picture to next picture in milliseconds")]
    [Required]
    public int GalleryTransitionDuration { get; private set; }

    /// <summary>
    /// Name of the transition animation 
    /// </summary>
    [GraphQLDescription("Name of the transition animation ")]
    [Required]
    public string GalleryTransitionType { get; private set; } = string.Empty;
    #endregion
    #endregion

    #region C'tor
    /// <summary>
    /// C'tor without parameters would be used by EF-Core
    /// </summary>
    private UserSetting()
    {

    }

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="userName">Username of user to which this setting belongs to</param>
    public UserSetting(string userName) : base()
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
        VideoAutoPlay = true;
        VideoAutoPlayOtherVideos = true;
        VideoLoop = false;
        VideoTimeToPlayNextVideo = 5;
        VideoVolume = 100;
        VideoTimeSeekForwardRewind = 10;
        QuestionKeepUploadWhenDelete = true;
        DefaultKeepUploadWhenDelete = true;
        ShowButtonForward = true;
        ShowButtonRewind = true;
        ShowZoomMenu = true;
        ShowPlayRateMenu = true;
        ShowMirrorButton = true;
        ShowQualityMenu = true;
        ShowZoomInfo = true;
        AllowZoomingWithMouseWheel = false;
        ShowTooltipForCurrentPlaytime = true;
        ShowTooltipForPlaytimeOnMouseCursor = true;
    }
    #endregion

    #region Domain Methods
    public void UpdateSettings(bool videoAutoPlay, int videoVolume, bool videoLoop,
        bool videoAutoPlayOtherVideos, int videoTimeToPlayNextVideo, int videoTimeSeekForwardRewind, bool galleryCloseEsc, bool galleryCloseDimmer,
        bool galleryMouseWheelChangeSlide, bool galleryShowThumbnails, bool galleryShowFullScreen, int galleryTransitionDuration,
        string galleryTransitionType, bool questionKeepUploadWhenDelete, bool defaultKeepUploadWhenDelete,
        bool showButtonForward, bool showButtonRewind, bool showZoomMenu, bool showPlayRateMenu, bool showMirrorButton,
        bool showQualityMenu, bool showZoomInfo, bool allowZoomingWithMouseWheel, bool showTooltipForCurrentPlaytime,
        bool showTooltipForPlaytimeOnMouseCursor)
    {
        if (videoVolume < 0 || videoVolume > 100)
        {
            throw new DomainException("Volume have to be between 0 and 100");
        }
        if (videoTimeToPlayNextVideo < 2 || videoTimeToPlayNextVideo > 10)
        {
            throw new DomainException("Time to play next video must be between 2 and 10");
        }
        if (videoTimeSeekForwardRewind != 5 && videoTimeSeekForwardRewind != 10 && videoTimeSeekForwardRewind != 20 &&
            videoTimeSeekForwardRewind != 30)
        {
            throw new DomainException("Seek time must be 5, 10, 20 or 30 seconds");
        }
        if (galleryTransitionDuration < 500 || galleryTransitionDuration > 900)
        {
            throw new DomainException("Gallery transition duration must be between 500 and 900");
        }
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
        VideoTimeSeekForwardRewind = videoTimeSeekForwardRewind;
        GalleryCloseEsc = galleryCloseEsc;
        GalleryCloseDimmer = galleryCloseDimmer;
        GalleryMouseWheelChangeSlide = galleryMouseWheelChangeSlide;
        GalleryShowThumbnails = galleryShowThumbnails;
        GalleryShowFullScreen = galleryShowFullScreen;
        GalleryTransitionDuration = galleryTransitionDuration;
        GalleryTransitionType = galleryTransitionType;
        QuestionKeepUploadWhenDelete = questionKeepUploadWhenDelete;
        DefaultKeepUploadWhenDelete = defaultKeepUploadWhenDelete;
        ShowButtonForward = showButtonForward;
        ShowButtonRewind = showButtonRewind;
        ShowZoomMenu = showZoomMenu;
        ShowPlayRateMenu = showPlayRateMenu;
        ShowMirrorButton = showMirrorButton;
        ShowQualityMenu = showQualityMenu;
        ShowZoomInfo = showZoomInfo;
        AllowZoomingWithMouseWheel = allowZoomingWithMouseWheel;
        ShowTooltipForCurrentPlaytime = showTooltipForCurrentPlaytime;
        ShowTooltipForPlaytimeOnMouseCursor = showTooltipForPlaytimeOnMouseCursor;
    }
    #endregion

    #region Called from Change-Tracker
    public override Task EntityModifiedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        AddDomainEvent(new DomainEventUserSettingChanged(Id));

        return Task.CompletedTask;
    }

    public override Task EntityAddedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        AddDomainEvent(new DomainEventUserSettingCreated(Id));

        return Task.CompletedTask;
    }

    public override Task EntityDeletedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        AddDomainEvent(new DomainEventUserSettingDeleted(Id));

        return Task.CompletedTask;
    }
    #endregion
}
