namespace FamilieLaissMassTransitDefinitions.Contracts.Events;

public interface IMassMediaItemDeletedEvent
{
    long Id { get; }

    bool IsPicture { get; }

    bool DeleteUploadItem { get; }

    long UploadItemId { get; }
}