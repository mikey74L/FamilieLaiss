using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadVideo;

namespace FamilieLaissMassTransitDefinitions.Events.UploadVideo;

public class MassVideoConvertedEvent : IMassVideoConvertedEvent
{
    public required long UploadVideoId { get; init; }
    public required long ConvertStatusId { get; init; }
}