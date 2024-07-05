using DomainHelper.DomainEvents;

namespace Upload.Domain.DomainEvents.UploadPortrait;

/// <summary>
/// Event for upload portrait created
/// </summary>
public class DomainEventUploadPortraitCreated : DomainEventSingle
{
    #region Properties
    /// <summary>
    /// Username this portrait picture belongs to
    /// </summary>
    public string UserName { get; }
    #endregion

    #region C'tor
    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for upload portrait</param>
    /// <param name="userName">Username this portrait picture belongs to</param>
    public DomainEventUploadPortraitCreated(long id, string userName) : base(id.ToString())
    {
        UserName = userName;
    }
    #endregion
}
