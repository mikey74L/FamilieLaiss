using DomainHelper.DomainEvents;

namespace Catalog.Domain.DomainEvents.Category;

/// <summary>
/// Event for category created
/// </summary>
public class DomainEventCategoryCreated : DomainEventSingle
{
    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for category</param>
    public DomainEventCategoryCreated(long id) : base(id.ToString())
    {
    }

    #endregion
}