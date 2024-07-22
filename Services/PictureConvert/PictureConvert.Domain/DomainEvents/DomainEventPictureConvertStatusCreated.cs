using DomainHelper.DomainEvents;

namespace PictureConvert.Domain.DomainEvents;

/// <summary>
/// Event for upload picture converted
/// </summary>
public class DomainEventPictureConvertStatusCreated : DomainEventSingle
{
    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for upload picture</param>
    public DomainEventPictureConvertStatusCreated(long id) : base(id.ToString())
    {
    }

    public required long UploadPictureId { get; init; }
    public required string OriginalFilename { get; init; }

    #endregion
}