using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadPicture;

namespace FamilieLaissMassTransitDefinitions.Events.UploadPicture;

public class MassUploadPictureDeletedEvent : IMassUploadPictureDeletedEvent
{
    /// <inheritdoc />
    public required long Id { get; init; }
}
