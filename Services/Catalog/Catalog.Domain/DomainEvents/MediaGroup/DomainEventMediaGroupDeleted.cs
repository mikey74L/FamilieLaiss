using DomainHelper.DomainEvents;

namespace Catalog.Domain.DomainEvents.MediaGroup;

/// <summary>
/// Event for media group deleted
/// </summary>
public class DomainEventMediaGroupDeleted : DomainEventSingle
{
    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for media item</param>
    public DomainEventMediaGroupDeleted(long id) : base(id.ToString())
    {
    }

    #endregion
}