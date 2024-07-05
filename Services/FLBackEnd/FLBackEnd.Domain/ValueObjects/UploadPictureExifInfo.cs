using DomainHelper.AbstractClasses;
using HotChocolate;
using System.ComponentModel.DataAnnotations;

namespace FLBackEnd.Domain.ValueObjects;

/// <summary>
/// Value-Object for Exif-Information to the upload picture
/// </summary>
[GraphQLDescription("Exif information to the upload picture")]
public class UploadPictureExifInfo : ValueObject
{
    #region Public Properties

    /// <summary>
    /// Make for the camera
    /// </summary>
    [MaxLength(300)]
    [GraphQLDescription("Make for the camera")]
    public string Make { get; set; } = string.Empty;

    /// <summary>
    /// Camera model
    /// </summary>
    [MaxLength(300)]
    [GraphQLDescription("Camera model")]
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Resolution x
    /// </summary>
    [GraphQLDescription("Resolution x")]
    public double? ResolutionX { get; set; }

    /// <summary>
    /// Resolution Y
    /// </summary>
    [GraphQLDescription("Resolution Y")]
    public double? ResolutionY { get; set; }

    /// <summary>
    /// The resolution unit
    /// </summary>
    [MaxLength(100)]
    [GraphQLDescription("The resolution unit")]
    public short? ResolutionUnit { get; set; }

    /// <summary>
    /// Orientation for picture
    /// </summary>
    [GraphQLDescription("Orientation for picture")]
    public short? Orientation { get; set; }

    /// <summary>
    /// Timestamp when picture was recorded
    /// </summary>
    [GraphQLDescription("Timestamp when picture was recorded")]
    public DateTimeOffset? DdlRecorded { get; set; }

    /// <summary>
    /// Exposure-Time 
    /// </summary>
    [MaxLength(50)]
    [GraphQLDescription("Exposure-Time ")]
    public double? ExposureTime { get; set; }

    /// <summary>
    /// Exposure program
    /// </summary>
    [GraphQLDescription("Exposure program")]
    public short? ExposureProgram { get; set; }

    /// <summary>
    /// Exposure mode
    /// </summary>
    [GraphQLDescription("Exposure mode")]
    public short? ExposureMode { get; set; }

    /// <summary>
    /// F-Number
    /// </summary>
    [GraphQLDescription("F-Number")]
    public double? FNumber { get; set; }

    /// <summary>
    /// ISO sensitivity
    /// </summary>
    [GraphQLDescription("ISO sensitivity")]
    public int? IsoSensitivity { get; set; }

    /// <summary>
    /// Shutter speed
    /// </summary>
    [GraphQLDescription("Shutter speed")]
    public double? ShutterSpeed { get; set; }

    /// <summary>
    /// Metering mode
    /// </summary>
    [GraphQLDescription("Metering mode")]
    public short? MeteringMode { get; set; }

    /// <summary>
    /// Flash mode
    /// </summary>
    [GraphQLDescription("Flash mode")]
    public short? FlashMode { get; set; }

    /// <summary>
    /// Focal length
    /// </summary>
    [GraphQLDescription("Focal length")]
    public double? FocalLength { get; set; }

    /// <summary>
    /// Sensing mode
    /// </summary>
    [GraphQLDescription("Sensing mode")]
    public short? SensingMode { get; set; }

    /// <summary>
    /// White-Balance mode
    /// </summary>
    [GraphQLDescription("White-Balance mode")]
    public short? WhiteBalanceMode { get; set; }

    /// <summary>
    /// Sharpness
    /// </summary>
    [GraphQLDescription("Sharpness")]
    public short? Sharpness { get; set; }

    /// <summary>
    /// Longitude for GPS-Position
    /// </summary>
    [GraphQLDescription("Longitude for GPS-Position")]
    public double? GpsLongitude { get; set; }

    /// <summary>
    /// Latitude for GPS-Position
    /// </summary>
    [GraphQLDescription("Latitude for GPS-Position")]
    public double? GpsLatitude { get; set; }

    /// <summary>
    /// Contrast
    /// </summary>
    [GraphQLDescription("Contrast")]
    public short? Contrast { get; set; }

    /// <summary>
    /// Saturation
    /// </summary>
    [GraphQLDescription("Saturation")]
    public short? Saturation { get; set; }

    #endregion

    #region C'tor

    /// <summary>
    /// Constructor
    /// </summary>
    private UploadPictureExifInfo()
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="make">Make for the camera</param>
    /// <param name="model">Camera model</param>
    /// <param name="resolutionX">Resolution X</param>
    /// <param name="resolutionY">Resolution Y</param>
    /// <param name="resolutionUnit">The resolution unit</param>
    /// <param name="orientation">Orientation for picture</param>
    /// <param name="ddlRecorded">Timestamp when picture was recorded</param>
    /// <param name="exposureTime">Exposure-Time</param>
    /// <param name="exposureProgram">Exposure program</param>
    /// <param name="exposureMode">Exposure mode</param>
    /// <param name="fNumber">F-Number</param>
    /// <param name="isoSensitivity">ISO sensitivity</param>
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
    public UploadPictureExifInfo(string make, string model, double? resolutionX, double? resolutionY,
        short? resolutionUnit,
        short? orientation, DateTimeOffset? ddlRecorded, double? exposureTime,
        short? exposureProgram, short? exposureMode, double? fNumber, int? isoSensitivity, double? shutterSpeed,
        short? meteringMode, short? flashMode, double? focalLength, short? sensingMode, short? whiteBalanceMode,
        short? sharpness, double? gpsLongitude, double? gpsLatitude, short? contrast, short? saturation)
    {
        Make = make;
        Model = model;
        ResolutionX = resolutionX;
        ResolutionY = resolutionY;
        ResolutionUnit = resolutionUnit;
        Orientation = orientation;
        DdlRecorded = ddlRecorded;
        ExposureTime = exposureTime;
        ExposureProgram = exposureProgram;
        ExposureMode = exposureMode;
        FNumber = fNumber;
        IsoSensitivity = isoSensitivity;
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

    [GraphQLIgnore]
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