using DomainHelper.DomainEvents;

namespace Upload.Domain.DomainEvents.UploadPortrait;

/// <summary>
/// Event for upload portrait created
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="id">Identifier for upload portrait</param>
/// <param name="userName">Username this portrait picture belongs to</param>
public class DomainEventUploadPortraitCreated(long id, string userName) : DomainEventSingle(id.ToString())
{
    #region Properties

    /// <summary>
    /// Username this portrait picture belongs to
    /// </summary>
    public string UserName { get; } = userName;

    #endregion
}