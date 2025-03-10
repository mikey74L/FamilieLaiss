namespace FamilieLaissMassTransitDefinitions.Contracts.Commands.UploadPicture;

public interface IMassSetUploadPictureDimensionsCmd
{
    long Id { get; set; }
    int Height { get; set; }
    int Width { get; set; }
}