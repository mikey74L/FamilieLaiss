using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadVideo;
using FamilieLaissMassTransitDefinitions.Events.UploadVideo;
using MassTransit;
using MediatR;
using Upload.Domain.DomainEvents.UploadVideo;

namespace Upload.API.DomainEvents.UploadVideo;

public class DomainEventUploadVideoDeletedHandler(
    ILogger<DomainEventUploadVideoDeletedHandler> logger,
    IBus massTransit) : INotificationHandler<DomainEventUploadVideoDeleted>
{
    public async Task Handle(DomainEventUploadVideoDeleted notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Mediatr-Notification-Handler for domain event upload video deleted was called: {Message}", notification);

        logger.LogDebug("Send event on message bus for upload video deleted");
        var newEvent = new MassUploadVideoDeletedEvent()
        {
            Id = long.Parse(notification.ID),
        };
        await massTransit.Publish<IMassUploadVideoDeletedEvent>(newEvent, cancellationToken);
    }
}