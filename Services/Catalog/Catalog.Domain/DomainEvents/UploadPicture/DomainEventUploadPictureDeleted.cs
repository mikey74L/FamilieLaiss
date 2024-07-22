using DomainHelper.DomainEvents;

namespace Catalog.Domain.DomainEvents.UploadPicture;

/// <summary>
/// Event for upload picture deleted
/// </summary>
public class DomainEventUploadPictureDeleted : DomainEventSingle
{
    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for upload picture</param>
    public DomainEventUploadPictureDeleted(long id) : base(id.ToString())
    {
    }

    #endregion
}