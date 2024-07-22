using FamilieLaissMassTransitDefinitions.Contracts.Events;

namespace FamilieLaissMassTransitDefinitions.Events;

public class MassVideoConvertedEvent : IMassVideoConvertedEvent
{
    public required long UploadVideoId { get; init; }
    public required long ConvertStatusId { get; init; }
}