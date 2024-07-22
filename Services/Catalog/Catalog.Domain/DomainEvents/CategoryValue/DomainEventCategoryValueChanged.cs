using DomainHelper.DomainEvents;

namespace Catalog.Domain.DomainEvents.CategoryValue;

/// <summary>
/// Event for category value changed
/// </summary>
public class DomainEventCategoryValueChanged : DomainEventSingle
{
    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for category value</param>
    public DomainEventCategoryValueChanged(long id) : base(id.ToString())
    {
    }

    #endregion
}