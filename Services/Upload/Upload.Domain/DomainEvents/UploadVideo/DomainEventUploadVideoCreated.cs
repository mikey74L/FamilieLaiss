using DomainHelper.DomainEvents;
using FamilieLaissSharedObjects.Enums;

namespace Upload.Domain.DomainEvents.UploadVideo;

/// <summary>
/// Event for upload video created
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="id">Identifier for upload video</param>
/// <param name="filename">Original filename of the uploaded video</param>
/// <param name="status">Status for the upload video</param>
public class DomainEventUploadVideoCreated(long id, string filename, EnumUploadStatus status)
    : DomainEventSingle(id.ToString())
{
    #region Properties

    /// <summary>
    /// Original filename of the uploaded video
    /// </summary>
    public string Filename { get; } = filename;

    /// <summary>
    /// Status for the upload video
    /// </summary>
    public EnumUploadStatus Status { get; } = status;

    #endregion
}