using FamilieLaissSharedObjects.Enums;

namespace FamilieLaissMassTransitDefinitions.Contracts.Commands;

public interface IMassSetVideoInfoDataCmd
{
    long Id { get; set; }
    EnumVideoType VideoType { get; set; }
    int Height { get; set; }
    int Width { get; set; }
    int Hours { get; set; }
    int Minutes { get; set; }
    int Seconds { get; set; }
    double? Longitude { get; set; }
    double? Latitude { get; set; }
}