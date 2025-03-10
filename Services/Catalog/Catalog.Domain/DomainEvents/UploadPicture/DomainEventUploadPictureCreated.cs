using DomainHelper.DomainEvents;

namespace Catalog.Domain.DomainEvents.UploadPicture;

/// <summary>
/// Event for upload picture created
/// </summary>
public class DomainEventUploadPictureCreated : DomainEventSingle
{
    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for upload picture</param>
    public DomainEventUploadPictureCreated(long id) : base(id.ToString())
    {
    }

    #endregion
}