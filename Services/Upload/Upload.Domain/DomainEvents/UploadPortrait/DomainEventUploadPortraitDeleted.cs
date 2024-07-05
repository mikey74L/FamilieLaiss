using DomainHelper.DomainEvents;

namespace Upload.Domain.DomainEvents.UploadPortrait;

/// <summary>
/// Event for upload portrait deleted
/// </summary>
public class DomainEventUploadPortraitDeleted : DomainEventSingle
{
    #region C'tor
    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for upload portrait</param>
    public DomainEventUploadPortraitDeleted(long id) : base(id.ToString())
    {
    }
    #endregion
}
