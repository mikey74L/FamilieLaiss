using DomainHelper.DomainEvents;
using FamilieLaissSharedObjects.Enums;
using Upload.Domain.ValueObjects;

namespace Upload.Domain.DomainEvents.UploadPicture;

/// <summary>
/// Event for upload picture changed
/// </summary>
public class DomainEventUploadPictureChanged : DomainEventSingle
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

    /// <summary>
    /// The original height of the picture
    /// </summary>
    public int? Height { get; private set; }

    /// <summary>
    /// The original width of the picture
    /// </summary>
    public int? Width { get; private set; }

    /// <summary>
    /// The Exif-Info for this Upload-Picture
    /// </summary>
    public UploadPictureExifInfo? UploadPictureExifInfo { get; private set; }

    /// <summary>
    /// Google-Geo-Coding-Adresses for this Upload-Picture as Value-Object
    /// </summary>
    public GoogleGeoCodingAddress? GoogleGeoCodingAdress { get; private set; }
    #endregion

    #region C'tor
    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for upload picture</param>
    /// <param name="fileName">The original filename for the picture file that was uploaded</param>
    /// <param name="status">Status for the upload picture</param>
    /// <param name="height">The original height of the picture</param>
    /// <param name="width">The original width of the pictureparam>
    /// <param name="googleGeoCodingAdress">Google-Geo-Coding-Adresses for this Upload-Picture as Value-Object</param>
    /// <param name="uploadPictureExifInfo">The Exif-Info for this Upload-Picture</param>
    public DomainEventUploadPictureChanged(long id, string fileName, EnumUploadStatus status,
        int? height, int? width, UploadPictureExifInfo? uploadPictureExifInfo,
        GoogleGeoCodingAddress? googleGeoCodingAdress) : base(id.ToString())
    {
        Filename = fileName;
        Status = status;
        Height = height;
        Width = width;
        UploadPictureExifInfo = uploadPictureExifInfo;
        GoogleGeoCodingAdress = googleGeoCodingAdress;
    }
    #endregion
}
