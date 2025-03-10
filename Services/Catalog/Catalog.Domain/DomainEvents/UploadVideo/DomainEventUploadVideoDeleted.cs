using DomainHelper.DomainEvents;

namespace Catalog.Domain.DomainEvents.UploadVideo;

/// <summary>
/// Event for upload video deleted
/// </summary>
public class DomainEventUploadVideoDeleted : DomainEventSingle
{
    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for upload video</param>
    public DomainEventUploadVideoDeleted(long id) : base(id.ToString())
    {
    }

    #endregion
}