using FamilieLaissSharedObjects.Enums;
using System;
using Upload.DTO.ValueData;

namespace Upload.DTO.UploadPicture;

/// <summary>
/// DTO-Class for querying upload pictures
/// </summary>
public class UploadPictureDto
{
    /// <summary>
    /// The unique identifier for the upload picture
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The original filename for the picture file that was uploaded
    /// </summary>
    public string Filename { get; set; } = string.Empty;

    /// <summary>
    /// The original height of the picture
    /// </summary>
    public int? Height { get; set; }

    /// <summary>
    /// The original width of the picture
    /// </summary>
    public int? Width { get; set; }

    /// <summary>
    /// Status for the upload picture
    /// </summary>
    public EnumUploadStatus Status { get; set; }

    /// <summary>
    /// The date and time when the upload picture was created
    /// </summary>
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// The Exif-Info for this Upload-Picture
    /// </summary>
    public UploadPictureExifInfoDto? UploadPictureExifInfo { get; set; }

    /// <summary>
    /// Google-Geo-Coding-Adresses for this Upload-Picture as Value-Object
    /// </summary>
    public GoogleGeoCodingAddressDto? GoogleGeoCodingAdress { get; set; }
}
