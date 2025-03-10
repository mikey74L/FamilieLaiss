using DomainHelper.DomainEvents;
using FamilieLaissSharedObjects.Enums;

namespace Catalog.Domain.DomainEvents.MediaItem;

/// <summary>
/// Event for media item created
/// </summary>
public class DomainEventMediaItemCreated : DomainEventSingle
{
    #region Properties

    /// <summary>
    /// Type for media item
    /// </summary>
    public EnumMediaType MediaType { get; }

    /// <summary>
    /// Unique identifier for assigned upload item
    /// </summary>
    public long UploadItemId { get; }

    #endregion

    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for media item</param>
    /// <param name="mediaType">Type for media item</param>
    /// <param name="uploadItemId">Unique identifier for assigned upload item</param>
    public DomainEventMediaItemCreated(long id, EnumMediaType mediaType, long uploadItemId) : base(id.ToString())
    {
        MediaType = mediaType;
        UploadItemId = uploadItemId;
    }

    #endregion
}