using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadVideo;
using FamilieLaissMassTransitDefinitions.Events.UploadVideo;
using MassTransit;
using MediatR;
using Upload.Domain.DomainEvents.UploadVideo;

namespace Upload.API.DomainEvents.UploadVideo;

public class DomainEventUploadVideoCreatedHandler(
    ILogger<DomainEventUploadVideoCreatedHandler> logger,
    IBus massTransit) : INotificationHandler<DomainEventUploadVideoCreated>
{
    public async Task Handle(DomainEventUploadVideoCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Mediatr-Notification-Handler for domain event upload video created was called: {Message}", notification);

        logger.LogDebug("Send event on message bus for upload video created");
        var newEvent = new MassVideoUploadedEvent()
        {
            Id = long.Parse(notification.ID),
            Filename = notification.Filename
        };
        await massTransit.Publish<IMassVideoUploadedEvent>(newEvent, cancellationToken);
    }
}