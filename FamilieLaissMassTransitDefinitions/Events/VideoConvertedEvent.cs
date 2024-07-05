using FamilieLaissMassTransitDefinitions.Contracts.Events;

namespace FamilieLaissMassTransitDefinitions.Events
{
    public class VideoConvertedEvent : IVideoConvertedEvent
    {
        public required long UploadVideoId { get; init; }
        public required long ConvertStatusId { get; init; }
    }
}
