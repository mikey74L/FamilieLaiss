using FamilieLaissMassTransitDefinitions.Contracts.Commands.UploadPicture;
using System;

namespace FamilieLaissMassTransitDefinitions.Commands.UploadPicture;

public class MassSetUploadPictureExifInfoCmd : IMassSetUploadPictureExifInfoCmd
{
    #region Implementation of IMassSetUploadPictureExifInfoCmd

    /// <inheritdoc />
    public long Id { get; set; }

    /// <inheritdoc />
    public string Make { get; set; }

    /// <inheritdoc />
    public string Model { get; set; }

    /// <inheritdoc />
    public double? ResolutionX { get; set; }

    /// <inheritdoc />
    public double? ResolutionY { get; set; }

    /// <inheritdoc />
    public short? ResolutionUnit { get; set; }

    /// <inheritdoc />
    public short? Orientation { get; set; }

    /// <inheritdoc />
    public DateTimeOffset? DdlRecorded { get; set; }

    /// <inheritdoc />
    public double? ExposureTime { get; set; }

    /// <inheritdoc />
    public short? ExposureProgram { get; set; }

    /// <inheritdoc />
    public short? ExposureMode { get; set; }

    /// <inheritdoc />
    public double? FNumber { get; set; }

    /// <inheritdoc />
    public int? IsoSensitivity { get; set; }

    /// <inheritdoc />
    public double? ShutterSpeed { get; set; }

    /// <inheritdoc />
    public short? MeteringMode { get; set; }

    /// <inheritdoc />
    public short? FlashMode { get; set; }

    /// <inheritdoc />
    public double? FocalLength { get; set; }

    /// <inheritdoc />
    public short? SensingMode { get; set; }

    /// <inheritdoc />
    public short? WhiteBalanceMode { get; set; }

    /// <inheritdoc />
    public short? Sharpness { get; set; }

    /// <inheritdoc />
    public double? GpsLongitude { get; set; }

    /// <inheritdoc />
    public double? GpsLatitude { get; set; }

    /// <inheritdoc />
    public short? Contrast { get; set; }

    /// <inheritdoc />
    public short? Saturation { get; set; }

    #endregion
}