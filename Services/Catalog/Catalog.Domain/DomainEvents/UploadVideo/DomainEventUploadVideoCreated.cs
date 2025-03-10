using DomainHelper.DomainEvents;

namespace Catalog.Domain.DomainEvents.UploadVideo;

/// <summary>
/// Event for upload video created
/// </summary>
public class DomainEventUploadVideoCreated : DomainEventSingle
{
    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for upload video</param>
    public DomainEventUploadVideoCreated(long id) : base(id.ToString())
    {
    }

    #endregion
}