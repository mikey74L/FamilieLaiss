using FamilieLaissMassTransitDefinitions.Contracts.Events.MediaItem;
using FamilieLaissSharedObjects.Enums;

namespace FamilieLaissMassTransitDefinitions.Events.MediaItem;

public class MassMediaItemDeletedEvent : IMassMediaItemDeletedEvent
{
    #region Properties

    /// <summary>
    /// Identifier for media item
    /// </summary>
    public required long Id { get; init; }

    /// <summary>
    /// Identifier for assigned upload item
    /// </summary>
    public required long UploadItemId { get; init; }

    /// <summary>
    /// Should upload item be deleted
    /// </summary>
    public required bool DeleteUploadItem { get; init; }

    /// <summary>
    /// Is media item a picture
    /// </summary>
    public required EnumMediaType MediaType { get; init; }

    #endregion
}