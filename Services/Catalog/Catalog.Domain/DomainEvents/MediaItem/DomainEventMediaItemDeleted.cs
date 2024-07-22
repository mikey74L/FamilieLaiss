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
    public EnumMediaType MediaType { get; init; }

    /// <summary>
    /// Identifier for assigned upload item
    /// </summary>
    public long UploadItemId { get; init; }

    #endregion

    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for media item</param>
    /// <param name="mediaType">Media type for media item</param>
    /// <param name="uploadItemId">Identifier for upload item</param>
    public DomainEventMediaItemDeleted(long id, EnumMediaType mediaType, long uploadItemId) :
        base(id.ToString())
    {
        MediaType = mediaType;
        UploadItemId = uploadItemId;
    }

    #endregion
}