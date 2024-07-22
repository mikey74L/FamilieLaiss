using Catalog.Domain.DomainEvents.MediaItem;
using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FamilieLaissMassTransitDefinitions.Events;
using MassTransit;
using MediatR;

namespace Catalog.API.DomainEvents.MediaItem;

public class DomainEventMediaItemCreatedHandler(
    ILogger<DomainEventMediaItemCreatedHandler> logger,
    IBus massTransit) : INotificationHandler<DomainEventMediaItemCreated>
{
    public async Task Handle(DomainEventMediaItemCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Mediatr-Notification-Handler for domain event media item created was called: {Message}", notification);

        logger.LogDebug("Send event on message bus for media item created");
        var newEvent = new MassMediaItemCreatedEvent()
        {
            Id = long.Parse(notification.ID),
            MediaType = notification.MediaType,
            UploadItemId = notification.UploadItemId,
        };

        await massTransit.Publish<IMassPictureUploadedEvent>(newEvent, cancellationToken);
    }
}