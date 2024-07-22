using DomainHelper.DomainEvents;

namespace Upload.Domain.DomainEvents.UploadPicture;

/// <summary>
/// Event for upload picture deleted
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="id">Identifier for upload picture</param>
public class DomainEventUploadPictureDeleted(long id) : DomainEventSingle(id.ToString())
{
}