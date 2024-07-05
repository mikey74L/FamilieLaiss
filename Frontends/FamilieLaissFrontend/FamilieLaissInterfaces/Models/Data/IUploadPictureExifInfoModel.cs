using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Attributes;
using FamilieLaissInterfaces.Enums;

namespace FamilieLaissInterfaces.Models.Data;

[GraphQlFilterInputType(typeof(UploadPictureExifInfoFilterInput))]
[GraphQlFilterGroup("UploadView", "MakeModel", 3)]
[GraphQlFilterGroup("UploadView", "Resolution", 4)]
[GraphQlFilterGroup("UploadView", "Exposure", 5)]
[GraphQlFilterGroup("UploadView", "ContrastSaturation", 6)]
[GraphQlFilterGroup("UploadView", "MeteringFlash", 7)]
[GraphQlFilterGroup("UploadView", "WhiteSharpness", 8)]
[GraphQlFilterGroup("UploadView", "OrientationSensing", 9)]
[GraphQlFilterGroup("UploadView", "ISOAndFNumber", 10)]
[GraphQlFilterGroup("UploadView", "ShutterFocal", 11)]
public interface IUploadPictureExifInfoModel : IBaseModel
{
    #region Properties
    /// <summary>
    /// Make for the camera
    /// </summary>
    [GraphQlFilter("UploadView", "MakeModel", 1, GraphQlFilterType.ValueListStringOnly)]
    public string? Make { get; set; }

    /// <summary>
    /// Camera model
    /// </summary>
    [GraphQlFilter("UploadView", "MakeModel", 2, GraphQlFilterType.ValueListStringOnly)]
    public string? Model { get; set; }

    /// <summary>
    /// Resolution x
    /// </summary>
    [GraphQlFilter("UploadView", "Resolution", 1, GraphQlFilterType.NumberRange)]
    public double? ResolutionX { get; set; }

    /// <summary>
    /// Resolution Y
    /// </summary>
    [GraphQlFilter("UploadView", "Resolution", 2, GraphQlFilterType.NumberRange)]
    public double? ResolutionY { get; set; }

    /// <summary>
    /// The resolution unit
    /// </summary>
    [GraphQlFilter("UploadView", "Resolution", 3, GraphQlFilterType.ValueListInt)]
    public short? ResolutionUnit { get; set; }

    /// <summary>
    /// Timestamp when picture was recorded
    /// </summary>
    [GraphQlFilter("UploadView", "DateValues", 2, GraphQlFilterType.DateRange)]
    public DateTimeOffset? DdlRecorded { get; set; }

    /// <summary>
    /// Exposure-Time 
    /// </summary>
    [GraphQlFilter("UploadView", "Exposure", 1, GraphQlFilterType.ValueListDouble)]
    public double? ExposureTime { get; set; }

    /// <summary>
    /// Exposure program
    /// </summary>
    [GraphQlFilter("UploadView", "Exposure", 2, GraphQlFilterType.ValueListInt)]
    public short? ExposureProgram { get; set; }

    /// <summary>
    /// Exposure mode
    /// </summary>
    [GraphQlFilter("UploadView", "Exposure", 3, GraphQlFilterType.ValueListInt)]
    public short? ExposureMode { get; set; }

    /// <summary>
    /// F-Number
    /// </summary>
    [GraphQlFilter("UploadView", "ISOAndFNumber", 1, GraphQlFilterType.ValueListDoubleOnly)]
    public double? FNumber { get; set; }

    /// <summary>
    /// ISO sensitivity
    /// </summary>
    [GraphQlFilter("UploadView", "ISOAndFNumber", 2, GraphQlFilterType.ValueListIntOnly)]
    public int? IsoSensitivity { get; set; }

    /// <summary>
    /// Shutter speed
    /// </summary>
    [GraphQlFilter("UploadView", "ShutterFocal", 1, GraphQlFilterType.ValueListDouble)]
    public double? ShutterSpeed { get; set; }

    /// <summary>
    /// Metering mode
    /// </summary>
    [GraphQlFilter("UploadView", "MeteringFlash", 1, GraphQlFilterType.ValueListInt)]
    public short? MeteringMode { get; set; }

    /// <summary>
    /// Flash mode
    /// </summary>
    [GraphQlFilter("UploadView", "MeteringFlash", 2, GraphQlFilterType.ValueListInt)]
    public short? FlashMode { get; set; }

    /// <summary>
    /// Focal length
    /// </summary>
    [GraphQlFilter("UploadView", "ShutterFocal", 2, GraphQlFilterType.ValueListDouble)]
    public double? FocalLength { get; set; }

    /// <summary>
    /// Orientation for picture
    /// </summary>
    [GraphQlFilter("UploadView", "OrientationSensing", 1, GraphQlFilterType.ValueListInt)]
    public short? Orientation { get; set; }

    /// <summary>
    /// Sensing mode
    /// </summary>
    [GraphQlFilter("UploadView", "OrientationSensing", 2, GraphQlFilterType.ValueListInt)]
    public short? SensingMode { get; set; }

    /// <summary>
    /// White-Balance mode
    /// </summary>
    [GraphQlFilter("UploadView", "WhiteSharpness", 1, GraphQlFilterType.ValueListInt)]
    public short? WhiteBalanceMode { get; set; }

    /// <summary>
    /// Sharpness
    /// </summary>
    [GraphQlFilter("UploadView", "WhiteSharpness", 2, GraphQlFilterType.ValueListInt)]
    public short? Sharpness { get; set; }

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

    [GraphQlFilter("UploadView", "ContrastSaturation", 1, GraphQlFilterType.ValueListInt)]
    public short? Contrast { get; set; }

    /// <summary>
    /// Saturation
    /// </summary>
    [GraphQlFilter("UploadView", "ContrastSaturation", 2, GraphQlFilterType.ValueListInt)]
    public short? Saturation { get; set; }
    #endregion

    #region Display Properties
    public string ResolutionXText { get; }

    public string ResolutionYText { get; }

    public string FNumberText { get; }

    public string FocalLengthText { get; }

    public string IsoSensitivityText { get; }

    public string OrientationText { get; }

    public string ExposureProgrammText { get; }

    public string ExposureModeText { get; }

    public string MeteringModeText { get; }

    public string FlashModeText { get; }

    public string SensingModeText { get; }

    public string WhiteBalanceModeText { get; }

    public string SharpnessText { get; }

    public string SaturationText { get; }

    public string ContrastText { get; }

    public string ShutterSpeedText { get; }

    public string ExposureTimeText { get; }
    #endregion
}
