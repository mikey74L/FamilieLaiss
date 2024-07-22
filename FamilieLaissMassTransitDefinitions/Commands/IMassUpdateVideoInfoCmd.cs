using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using FamilieLaissSharedObjects.Enums;

namespace FamilieLaissMassTransitDefinitions.Commands;

public class MassSetVideoInfoDataCmd : IMassSetVideoInfoDataCmd
{
    #region Implementation of IMassSetVideoInfoDataCmd

    /// <inheritdoc />
    public long Id { get; set; }

    /// <inheritdoc />
    public EnumVideoType VideoType { get; set; }

    /// <inheritdoc />
    public int Height { get; set; }

    /// <inheritdoc />
    public int Width { get; set; }

    /// <inheritdoc />
    public int Hours { get; set; }

    /// <inheritdoc />
    public int Minutes { get; set; }

    /// <inheritdoc />
    public int Seconds { get; set; }

    /// <inheritdoc />
    public double? Longitude { get; set; }

    /// <inheritdoc />
    public double? Latitude { get; set; }

    #endregion
}