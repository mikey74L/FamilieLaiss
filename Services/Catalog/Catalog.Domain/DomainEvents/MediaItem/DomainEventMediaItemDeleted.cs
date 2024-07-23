using DomainHelper.DomainEvents;
using FamilieLaissSharedObjects.Enums;

namespace Catalog.Domain.DomainEvents.MediaItem;

/// <summary>
/// Event for media item deleted
/// </summary>
public class DomainEventMediaItemDeleted : DomainEventSingle
{
    #region Properties
    /// <summary>
    /// Media-Type for this media item
    /// </summary>
    public required bool DeleteUploadItem { get; init; }

    /// <summary>
    /// Media-Type for this media item
    /// </summary>
    public required EnumMediaType MediaType { get; init; }

    /// <summary>
    /// Identifier for assigned upload item
    /// </summary>
    public required long UploadItemId { get; init; }

    #endregion

    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for media item</param>
    public DomainEventMediaItemDeleted(long id) :
        base(id.ToString())
    {
    }

    #endregion
}