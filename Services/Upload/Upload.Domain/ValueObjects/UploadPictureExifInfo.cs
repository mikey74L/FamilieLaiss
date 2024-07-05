using DomainHelper.AbstractClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Upload.Domain.ValueObjects;

/// <summary>
/// Value-Object for Exif-Information to the upload picture
/// </summary>
public class UploadPictureExifInfo : ValueObject
{
    #region Public Properties

    /// <summary>
    /// Make for the camera
    /// </summary>
    [MaxLength(300)]
    public string Make { get; private set; } = string.Empty;

    /// <summary>
    /// Camera model
    /// </summary>
    [MaxLength(300)]
    public string Model { get; private set; } = string.Empty;

    /// <summary>
    /// Resolution x
    /// </summary>
    public double? ResolutionX { get; private set; }

    /// <summary>
    /// Resolution Y
    /// </summary>
    public double? ResolutionY { get; private set; }

    /// <summary>
    /// The resolution unit
    /// </summary>
    [MaxLength(100)]
    public string ResolutionUnit { get; private set; } = string.Empty;

    /// <summary>
    /// Orientation for picture
    /// </summary>
    public Int16? Orientation { get; private set; }

    /// <summary>
    /// Timestamp when picture was recorded
    /// </summary>
    public DateTimeOffset? DdlRecorded { get; private set; }

    /// <summary>
    /// Exposure-Time 
    /// </summary>
    [MaxLength(50)]
    public string ExposureTime { get; private set; } = string.Empty;

    /// <summary>
    /// Exposure program
    /// </summary>
    public Int16? ExposureProgram { get; private set; }

    /// <summary>
    /// Exposure mode
    /// </summary>
    public Int16? ExposureMode { get; private set; }

    /// <summary>
    /// F-Number
    /// </summary>
    public double? FNumber { get; private set; }

    /// <summary>
    /// ISO sensitivity
    /// </summary>
    public int? IsoSensitivity { get; private set; }

    /// <summary>
    /// Shutter speed
    /// </summary>
    public double? ShutterSpeed { get; private set; }

    /// <summary>
    /// Metering mode
    /// </summary>
    public Int16? MeteringMode { get; private set; }

    /// <summary>
    /// Flash mode
    /// </summary>
    public Int16? FlashMode { get; private set; }

    /// <summary>
    /// Focal length
    /// </summary>
    public double? FocalLength { get; private set; }

    /// <summary>
    /// Sensing mode
    /// </summary>
    public Int16? SensingMode { get; private set; }

    /// <summary>
    /// White-Balance mode
    /// </summary>
    public Int16? WhiteBalanceMode { get; private set; }

    /// <summary>
    /// Sharpness
    /// </summary>
    public Int16? Sharpness { get; private set; }

    /// <summary>
    /// Longitude for GPS-Position
    /// </summary>
    public double? GpsLongitude { get; private set; }

    /// <summary>
    /// Latitude for GPS-Position
    /// </summary>
    public double? GpsLatitude { get; private set; }

    /// <summary>
    /// Contrast
    /// </summary>
    public Int16? Contrast { get; private set; }

    /// <summary>
    /// Saturation
    /// </summary>
    public Int16? Saturation { get; private set; }
    #endregion

    #region C'tor
    /// <summary>
    /// C'tor without parameters would be used by EF-Core
    /// </summary>
    private UploadPictureExifInfo()
    {
    }

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="make">Make for the camera</param>
    /// <param name="model">Camera model</param>
    /// <param name="resolutionX">Resolution X</param>
    /// <param name="resolutionY">Resolution Y</param>
    /// <param name="resolutionUnit">The resolution unit</param>
    /// <param name="orientation">Orientation for picture</param>
    /// <param name="ddlRecorded">Timestamp when picture was recorded</param>
    /// <param name="exposureTime">Exposure-Time</param>
    /// <param name="exposureProgramm">Exposure program</param>
    /// <param name="exposureMode">Exposure mode</param>
    /// <param name="fNumber">F-Number</param>
    /// <param name="isoSenstivity">ISO sensitivity</param>
    /// <param name="shutterSpeed">Shutter speed</param>
    /// <param name="meteringMode">Metering mode</param>
    /// <param name="flashMode">Flash mode</param>
    /// <param name="focalLength">Focal length</param>
    /// <param name="sensingMode">Sensing mode</param>
    /// <param name="whiteBalanceMode">White-Balance mode</param>
    /// <param name="sharpness">Sharpness</param>
    /// <param name="gpsLongitude">Longitude for GPS-Position</param>
    /// <param name="gpsLatitude">Latitude for GPS-Position</param>
    /// <param name="contrast">Contrast</param>
    /// <param name="saturation">Saturation</param>
    public UploadPictureExifInfo(string make, string model, double? resolutionX, double? resolutionY, string resolutionUnit,
        Int16? orientation, DateTimeOffset? ddlRecorded, string exposureTime,
        Int16? exposureProgramm, Int16? exposureMode, double? fNumber, int? isoSenstivity, double? shutterSpeed,
        Int16? meteringMode, Int16? flashMode, double? focalLength, Int16? sensingMode, Int16? whiteBalanceMode,
        Int16? sharpness, double? gpsLongitude, double? gpsLatitude, Int16? contrast, Int16? saturation)
    {
        Make = make;
        Model = model;
        ResolutionX = resolutionX;
        ResolutionY = resolutionY;
        ResolutionUnit = resolutionUnit;
        Orientation = orientation;
        DdlRecorded = ddlRecorded;
        ExposureTime = exposureTime;
        ExposureProgram = exposureProgramm;
        ExposureMode = exposureMode;
        FNumber = fNumber;
        IsoSensitivity = isoSenstivity;
        ShutterSpeed = shutterSpeed;
        MeteringMode = meteringMode;
        FlashMode = flashMode;
        FocalLength = focalLength;
        SensingMode = sensingMode;
        WhiteBalanceMode = whiteBalanceMode;
        Sharpness = sharpness;
        GpsLongitude = gpsLongitude;
        GpsLatitude = gpsLatitude;
        Contrast = contrast;
        Saturation = saturation;
    }
    #endregion

    #region Protected Overrides
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Make;
        yield return Model;
        yield return ResolutionX;
        yield return ResolutionY;
        yield return ResolutionUnit;
        yield return Orientation;
        yield return DdlRecorded;
        yield return ExposureTime;
        yield return ExposureProgram;
        yield return ExposureMode;
        yield return FNumber;
        yield return IsoSensitivity;
        yield return ShutterSpeed;
        yield return MeteringMode;
        yield return FlashMode;
        yield return FocalLength;
        yield return SensingMode;
        yield return WhiteBalanceMode;
        yield return Sharpness;
        yield return GpsLongitude;
        yield return GpsLatitude;
        yield return Contrast;
        yield return Saturation;
    }
    #endregion
}
