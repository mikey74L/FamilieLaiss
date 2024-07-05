using System.ComponentModel.DataAnnotations;

namespace FLBackEnd.API.GraphQL.Mutations.UserSetting;

public class UpdateUserSettingInput
{
    [GraphQLDescription("The id for the user")]
    public string Id { get; set; } = string.Empty;

    #region Media Item
    [GraphQLDescription("Should the question to keep the upload item be shown?")]
    public bool QuestionKeepUploadWhenDelete { get; private set; }

    [GraphQLDescription("What should be the default when question would not be shown")]
    public bool DefaultKeepUploadWhenDelete { get; private set; }
    #endregion

    #region Video-Player
    [GraphQLDescription("Should videos automatically start playing when video player is shown")]
    public bool VideoAutoPlay { get; private set; }

    [GraphQLDescription("The default volume vor video player all videos start playing with")]
    public int VideoVolume { get; private set; }

    [GraphQLDescription("Should videos play in a loop")]
    public bool VideoLoop { get; private set; }

    [GraphQLDescription("Should other videos of current album play after current video is fnished")]
    public bool VideoAutoPlayOtherVideos { get; private set; }

    [GraphQLDescription("Wait time to play other video in of current album in seconds")]
    [Required]
    public int VideoTimeToPlayNextVideo { get; private set; }

    [GraphQLDescription("Show zoom info in video player")]
    public bool ShowZoomInfo { get; private set; }

    [GraphQLDescription("Allow zoom with mouse wheel in video player")]
    public bool AllowZoomingWithMouseWheel { get; private set; }

    [GraphQLDescription("Show tooltip for playtime in video player when mouse over")]
    public bool ShowTooltipForPlaytimeOnMouseCursor { get; private set; }

    [GraphQLDescription("Show tooltip for current playtime in video player")]
    public bool ShowTooltipForCurrentPlaytime { get; private set; }

    [GraphQLDescription("Should the seek forward button be shown in the player")]
    public bool ShowButtonForward { get; private set; }

    [GraphQLDescription("Should the seek backward button be shown in the player")]
    public bool ShowButtonRewind { get; private set; }

    [GraphQLDescription("Should the player show the zoom menu")]
    public bool ShowZoomMenu { get; private set; }

    [GraphQLDescription("Show play rate menu in video player")]
    public bool ShowPlayRateMenu { get; private set; }

    [GraphQLDescription("Show mirror button in video player")]
    public bool ShowMirrorButton { get; private set; }

    [GraphQLDescription("Show quality menu in video player")]
    public bool ShowQualityMenu { get; private set; }

    [GraphQLDescription("Time in seconds the seek forward / rewind button will jump ahead / back in the video")]
    public int VideoTimeSeekForwardRewind { get; private set; }
    #endregion

    #region Slide-Show
    [GraphQLDescription("Close gallery with pressing the Escape-Key")]
    public bool GalleryCloseEsc { get; private set; }

    [GraphQLDescription("Close gallery with clicking on background")]
    public bool GalleryCloseDimmer { get; private set; }

    [GraphQLDescription("Change current photo in gallery by using the mouse wheel")]
    public bool GalleryMouseWheelChangeSlide { get; private set; }

    [GraphQLDescription("Show thumbnails from begin on")]
    public bool GalleryShowThumbnails { get; private set; }

    [GraphQLDescription("Start gallery in full screen mode")]
    public bool GalleryShowFullScreen { get; private set; }

    [GraphQLDescription("Time to transition from current picture to next picture in milliseconds")]
    public int GalleryTransitionDuration { get; private set; }

    [GraphQLDescription("Name of the transition animation ")]
    public string GalleryTransitionType { get; private set; } = string.Empty;
    #endregion
}
