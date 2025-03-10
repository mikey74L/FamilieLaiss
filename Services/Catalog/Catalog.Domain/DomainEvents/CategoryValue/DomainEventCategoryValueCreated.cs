using DomainHelper.DomainEvents;

namespace Catalog.Domain.DomainEvents.CategoryValue;

/// <summary>
/// Event for category value created
/// </summary>
public class DomainEventCategoryValueCreated : DomainEventSingle
{
    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for category value</param>
    public DomainEventCategoryValueCreated(long id) : base(id.ToString())
    {
    }

    #endregion
}