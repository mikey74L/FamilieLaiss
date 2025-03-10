using DomainHelper.DomainEvents;

namespace Catalog.Domain.DomainEvents.MediaGroup;

/// <summary>
/// Event for media group created
/// </summary>
public class DomainEventMediaGroupCreated : DomainEventSingle
{
    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for media item</param>
    public DomainEventMediaGroupCreated(long id) : base(id.ToString())
    {
    }

    #endregion
}