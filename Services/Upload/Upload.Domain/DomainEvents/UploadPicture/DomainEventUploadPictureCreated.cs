using DomainHelper.DomainEvents;
using FamilieLaissSharedObjects.Enums;

namespace Upload.Domain.DomainEvents.UploadPicture;

/// <summary>
/// Event for upload picture created
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="id">Identifier for upload picture</param>
/// <param name="fileName">The original filename for the picture file that was uploaded</param>
/// <param name="status">Status for the upload picture</param>
public class DomainEventUploadPictureCreated(long id, string fileName, EnumUploadStatus status)
    : DomainEventSingle(id.ToString())
{
    #region Properties

    /// <summary>
    /// The original filename for the picture file that was uploaded
    /// </summary>
    public string Filename { get; } = fileName;

    /// <summary>
    /// Status for the upload picture
    /// </summary>
    public EnumUploadStatus Status { get; } = status;

    #endregion
}