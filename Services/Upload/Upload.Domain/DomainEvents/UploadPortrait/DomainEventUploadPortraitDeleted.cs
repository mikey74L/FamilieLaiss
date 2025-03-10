using DomainHelper.DomainEvents;

namespace Upload.Domain.DomainEvents.UploadPortrait;

/// <summary>
/// Event for upload portrait deleted
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="id">Identifier for upload portrait</param>
public class DomainEventUploadPortraitDeleted(long id) : DomainEventSingle(id.ToString())
{
}