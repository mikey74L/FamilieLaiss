using DomainHelper.DomainEvents;
using FamilieLaissSharedObjects.Enums;

namespace Catalog.Domain.DomainEvents.MediaItem;

/// <summary>
/// Event for media item changed
/// </summary>
public class DomainEventMediaItemChanged : DomainEventSingle
{
    #region Properties

    /// <summary>
    /// Type for media item
    /// </summary>
    public EnumMediaType MediaType { get; }

    #endregion

    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for media item</param>
    /// <param name="mediaType">Type for media item</param>
    public DomainEventMediaItemChanged(long id, EnumMediaType mediaType) : base(id.ToString())
    {
        MediaType = mediaType;
    }

    #endregion
}