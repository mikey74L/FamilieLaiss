using DomainHelper.DomainEvents;
using FamilieLaissSharedObjects.Enums;

namespace Upload.Domain.DomainEvents.UploadVideo;

/// <summary>
/// Event for upload video created
/// </summary>
public class DomainEventUploadVideoCreated : DomainEventSingle
{
    #region Properties
    /// <summary>
    /// Original filename of the uploaded video
    /// </summary>
    public string Filename { get; }

    /// <summary>
    /// Status for the upload video
    /// </summary>
    public EnumUploadStatus Status { get; }
    #endregion

    #region C'tor
    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for upload video</param>
    /// <param name="filename">Original filename of the uploaded video</param>
    /// <param name="status">Status for the upload video</param>
    public DomainEventUploadVideoCreated(long id, string filename, EnumUploadStatus status) : base(id.ToString())
    {
        Filename = filename;
        Status = status;
    }
    #endregion
}
