using DomainHelper.DomainEvents;
using FamilieLaissSharedObjects.Enums;
using Upload.Domain.ValueObjects;

namespace Upload.Domain.DomainEvents.UploadVideo;

/// <summary>
/// Event for upload video changed
/// </summary>
public class DomainEventUploadVideoChanged : DomainEventSingle
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

    /// <summary>
    /// Is video a streaming video (MPEG-DASH)
    /// </summary>
    public EnumVideoType? VideoType { get; }

    /// <summary>
    /// Height of the original video
    /// </summary>
    public int? Height { get; }

    /// <summary>
    /// Width of the original video
    /// </summary>
    public int? Width { get; }

    /// <summary>
    /// The hour part for the duration of this video
    /// </summary>
    public int? Duration_Hour { get; }

    /// <summary>
    /// The minute part for the duration of this video
    /// </summary>
    public int? Duration_Minute { get; }

    /// <summary>
    /// The second part for the duration of this video
    /// </summary>
    public int? Duration_Second { get; }

    /// <summary>
    /// Longitude for GPS-Position
    /// </summary>
    public double? GPS_Longitude { get; }

    /// <summary>
    /// Latitude for GPS-Position
    /// </summary>
    public double? GPS_Latitude { get; }

    /// <summary>
    /// Google-Geo-Coding-Adresses for this Upload-Video as Value-Object
    /// </summary>
    public GoogleGeoCodingAddress? GoogleGeoCodingAdress { get; }
    #endregion

    #region C'tor
    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for upload video</param>
    /// <param name="filename">Original filename of the uploaded video</param>
    /// <param name="status">Status for the upload video</param>
    /// <param name="videoType">Is video a streaming video (MPEG-DASH)</param>
    /// <param name="height">Height of the original video</param>
    /// <param name="width">Width of the original video</param>
    /// <param name="durationHour">The hour part for the duration of this video</param>
    /// <param name="durationMinute">The minute part for the duration of this video</param>
    /// <param name="durationSecond">The second part for the duration of this video</param>
    /// <param name="gpsLatitude">Latitude for GPS-Position</param>
    /// <param name="gpsLongitude">Longitude for GPS-Position</param>
    /// <param name="geoCodingAdress">Google-Geo-Coding-Adresses for this Upload-Video as Value-Object</param>
    public DomainEventUploadVideoChanged(long id, string filename, EnumUploadStatus status, EnumVideoType? videoType,
        int? height, int? width, int? durationHour, int? durationMinute, int? durationSecond,
        double? gpsLongitude, double? gpsLatitude, GoogleGeoCodingAddress? geoCodingAdress) : base(id.ToString())
    {
        Filename = filename;
        Status = status;
        VideoType = videoType;
        Height = height;
        Width = width;
        Duration_Minute = durationHour;
        Duration_Minute = durationMinute;
        Duration_Second = durationSecond;
        GPS_Longitude = gpsLongitude;
        GPS_Latitude = gpsLatitude;
        GoogleGeoCodingAdress = geoCodingAdress;
    }
    #endregion
}
