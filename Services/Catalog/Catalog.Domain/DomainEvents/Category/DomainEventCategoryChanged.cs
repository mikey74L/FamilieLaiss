using DomainHelper.DomainEvents;

namespace Catalog.Domain.DomainEvents.Category;

/// <summary>
/// Event for category changed
/// </summary>
public class DomainEventCategoryChanged : DomainEventSingle
{
    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for category</param>
    public DomainEventCategoryChanged(long id) : base(id.ToString())
    {
    }

    #endregion
}