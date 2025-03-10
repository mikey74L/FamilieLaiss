using DomainHelper.DomainEvents;

namespace VideoConvert.Domain.DomainEvents;

/// <summary>
/// Event for upload video converted
/// </summary>
public class DomainEventVideoConvertStatusCreated : DomainEventSingle
{
    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for upload video</param>
    public DomainEventVideoConvertStatusCreated(long id) : base(id.ToString())
    {
    }

    public required long UploadVideoId { get; init; }
    public required string OriginalFilename { get; init; }

    #endregion
}