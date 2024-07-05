using DomainHelper.DomainEvents;
using FamilieLaissSharedObjects.Enums;

namespace Upload.Domain.DomainEvents.UploadVideo;

/// <summary>
/// Event for upload video deleted
/// </summary>
public class DomainEventUploadVideoDeleted : DomainEventSingle
{
    #region Properties
    /// <summary>
    /// The type of video
    /// </summary>
    public EnumVideoType? VideoType { get; init; }
    #endregion

    #region C'tor
    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for upload video</param>
    /// <param name="videoType">The type of video</param>
    public DomainEventUploadVideoDeleted(long id, EnumVideoType? videoType) : base(id.ToString())
    {
        VideoType = videoType;
    }
    #endregion
}
