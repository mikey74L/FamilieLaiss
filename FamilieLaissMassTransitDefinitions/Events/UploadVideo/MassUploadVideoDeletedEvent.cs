using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadVideo;

namespace FamilieLaissMassTransitDefinitions.Events.UploadVideo;

public class MassUploadVideoDeletedEvent : IMassUploadVideoDeletedEvent
{
    /// <inheritdoc />
    public required long Id { get; init; }
}
