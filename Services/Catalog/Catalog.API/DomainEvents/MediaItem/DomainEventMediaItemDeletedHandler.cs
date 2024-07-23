using Catalog.Domain.DomainEvents.MediaItem;
using FamilieLaissMassTransitDefinitions.Contracts.Events.MediaItem;
using FamilieLaissMassTransitDefinitions.Events.MediaItem;
using MassTransit;
using MediatR;

namespace Catalog.API.DomainEvents.MediaItem;

public class DomainEventMediaItemDeletedHandler(
    ILogger<DomainEventMediaItemDeletedHandler> logger,
    IBus massTransit) : INotificationHandler<DomainEventMediaItemDeleted>
{
    public async Task Handle(DomainEventMediaItemDeleted notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Mediatr-Notification-Handler for domain event media item deleted was called: {Message}", notification);

        logger.LogDebug("Send event on message bus for media item created");
        var newEvent = new MassMediaItemDeletedEvent()
        {
            Id = long.Parse(notification.ID),
            MediaType = notification.MediaType,
            UploadItemId = notification.UploadItemId,
            DeleteUploadItem = notification.DeleteUploadItem
        };

        await massTransit.Publish<IMassMediaItemDeletedEvent>(newEvent, cancellationToken);
    }
}