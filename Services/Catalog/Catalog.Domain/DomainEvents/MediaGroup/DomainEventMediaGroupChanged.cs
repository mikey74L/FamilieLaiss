using DomainHelper.DomainEvents;

namespace Catalog.Domain.DomainEvents.MediaGroup;

/// <summary>
/// Event for media group changed
/// </summary>
public class DomainEventMediaGroupChanged : DomainEventSingle
{
    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for media item</param>
    public DomainEventMediaGroupChanged(long id) : base(id.ToString())
    {
    }

    #endregion
}