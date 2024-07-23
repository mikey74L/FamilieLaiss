using FamilieLaissSharedObjects.Enums;

namespace FamilieLaissMassTransitDefinitions.Contracts.Events.MediaItem;

public interface IMassMediaItemDeletedEvent
{
    long Id { get; }

    EnumMediaType MediaType { get; }

    bool DeleteUploadItem { get; }

    long UploadItemId { get; }
}