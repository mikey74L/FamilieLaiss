using DomainHelper.DomainEvents;

namespace Catalog.Domain.DomainEvents.CategoryValue;

/// <summary>
/// Event for category value deleted
/// </summary>
public class DomainEventCategoryValueDeleted : DomainEventSingle
{
    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for category value</param>
    public DomainEventCategoryValueDeleted(long id) : base(id.ToString())
    {
    }

    #endregion
}