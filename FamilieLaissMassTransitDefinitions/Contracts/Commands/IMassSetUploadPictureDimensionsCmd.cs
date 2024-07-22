namespace FamilieLaissMassTransitDefinitions.Contracts.Commands;

public interface IMassSetUploadPictureDimensionsCmd
{
    long Id { get; set; }
    int Height { get; set; }
    int Width { get; set; }
}