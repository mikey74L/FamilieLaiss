using System;

namespace Upload.DTO.UploadPicture;

/// <summary>
/// DTO-Class for upload picture exif info
/// </summary>
public class UploadPictureExifInfoDto
{
    /// <summary>
    /// Make for the camera
    /// </summary>
    public string Make { get; set; } = string.Empty;

    /// <summary>
    /// Camera model
    /// </summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Resolution x
    /// </summary>
    public double? ResolutionX { get; set; }

    /// <summary>
    /// Resolution Y
    /// </summary>
    public double? ResolutionY { get; set; }

    /// <summary>
    /// The resolution unit
    /// </summary>
    public string ResolutionUnit { get; set; } = string.Empty;

    /// <summary>
    /// Orientation for picture
    /// </summary>
    public Int16? Orientation { get; set; }

    /// <summary>
    /// Timestamp when picture was recorded
    /// </summary>
    public DateTimeOffset? DdlRecorded { get; set; }

    /// <summary>
    /// Exposure-Time 
    /// </summary>
    public string ExposureTime { get; set; } = string.Empty;

    /// <summary>
    /// Exposure program
    /// </summary>
    public Int16? ExposureProgram { get; set; }

    /// <summary>
    /// Exposure mode
    /// </summary>
    public Int16? ExposureMode { get; set; }

    /// <summary>
    /// F-Number
    /// </summary>
    public double? FNumber { get; set; }

    /// <summary>
    /// ISO sensitivity
    /// </summary>
    public int? IsoSensitivity { get; set; }

    /// <summary>
    /// Shutter speed
    /// </summary>
    public double? ShutterSpeed { get; set; }

    /// <summary>
    /// Metering mode
    /// </summary>
    public Int16? MeteringMode { get; set; }

    /// <summary>
    /// Flash mode
    /// </summary>
    public Int16? FlashMode { get; set; }

    /// <summary>
    /// Focal length
    /// </summary>
    public double? FocalLength { get; set; }

    /// <summary>
    /// Sensing mode
    /// </summary>
    public Int16? SensingMode { get; set; }

    /// <summary>
    /// White-Balance mode
    /// </summary>
    public Int16? WhiteBalanceMode { get; set; }

    /// <summary>
    /// Sharpness
    /// </summary>
    public Int16? Sharpness { get; set; }

    /// <summary>
    /// Longitude for GPS-Position
    /// </summary>
    public double? GpsLongitude { get; set; }

    /// <summary>
    /// Latitude for GPS-Position
    /// </summary>
    public double? GpsLatitude { get; set; }

    /// <summary>
    /// Contrast
    /// </summary>
    public Int16? Contrast { get; set; }

    /// <summary>
    /// Saturation
    /// </summary>
    public Int16? Saturation { get; set; }
}
