using DomainHelper.DomainEvents;
using FamilieLaissSharedObjects.Enums;

namespace Upload.Domain.DomainEvents.UploadVideo;

/// <summary>
/// Event for upload video deleted
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="id">Identifier for upload video</param>
/// <param name="videoType">The type of video</param>
public class DomainEventUploadVideoDeleted(long id, EnumVideoType? videoType) : DomainEventSingle(id.ToString())
{
    #region Properties

    /// <summary>
    /// The type of video
    /// </summary>
    public EnumVideoType? VideoType { get; } = videoType;

    #endregion
}