using System;

namespace FamilieLaissMassTransitDefinitions.Contracts.Commands.UploadPicture;

public interface IMassSetUploadPictureExifInfoCmd
{
    long Id { get; set; }
    string Make { get; set; }
    string Model { get; set; }
    double? ResolutionX { get; set; }
    double? ResolutionY { get; set; }
    short? ResolutionUnit { get; set; }
    short? Orientation { get; set; }
    DateTimeOffset? DdlRecorded { get; set; }
    double? ExposureTime { get; set; }
    short? ExposureProgram { get; set; }
    short? ExposureMode { get; set; }
    double? FNumber { get; set; }
    int? IsoSensitivity { get; set; }
    double? ShutterSpeed { get; set; }
    short? MeteringMode { get; set; }
    short? FlashMode { get; set; }
    double? FocalLength { get; set; }
    short? SensingMode { get; set; }
    short? WhiteBalanceMode { get; set; }
    short? Sharpness { get; set; }
    double? GpsLongitude { get; set; }
    double? GpsLatitude { get; set; }
    short? Contrast { get; set; }
    short? Saturation { get; set; }
}