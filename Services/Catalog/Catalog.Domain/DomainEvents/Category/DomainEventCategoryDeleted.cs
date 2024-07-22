using DomainHelper.DomainEvents;

namespace Catalog.Domain.DomainEvents.Category;

/// <summary>
/// Event for category deleted
/// </summary>
public class DomainEventCategoryDeleted : DomainEventSingle
{
    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for category</param>
    public DomainEventCategoryDeleted(long id) : base(id.ToString())
    {
    }

    #endregion
}