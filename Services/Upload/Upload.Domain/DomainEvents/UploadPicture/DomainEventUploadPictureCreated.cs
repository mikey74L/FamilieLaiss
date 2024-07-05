using DomainHelper.DomainEvents;
using FamilieLaissSharedObjects.Enums;

namespace Upload.Domain.DomainEvents.UploadPicture;

/// <summary>
/// Event for upload picture created
/// </summary>
public class DomainEventUploadPictureCreated : DomainEventSingle
{
    #region Properties
    /// <summary>
    /// The original filename for the picture file that was uploaded
    /// </summary>
    public string Filename { get; }

    /// <summary>
    /// Status for the upload picture
    /// </summary>
    public EnumUploadStatus Status { get; }
    #endregion

    #region C'tor
    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for upload picture</param>
    /// <param name="fileName">The original filename for the picture file that was uploaded</param>
    /// <param name="status">Status for the upload picture</param>
    public DomainEventUploadPictureCreated(long id, string fileName, EnumUploadStatus status) : base(id.ToString())
    {
        Filename = fileName;
        Status = status;
    }
    #endregion
}
