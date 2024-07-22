using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FamilieLaissMassTransitDefinitions.Events;
using MassTransit;
using MediatR;
using Upload.Domain.DomainEvents.UploadPicture;

namespace Upload.API.DomainEvents;

public class DomainEventUploadPictureCreatedHandler(
    ILogger<DomainEventUploadPictureCreatedHandler> logger,
    IBus massTransit) : INotificationHandler<DomainEventUploadPictureCreated>
{
    public async Task Handle(DomainEventUploadPictureCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Mediatr-Notification-Handler for domain event upload picture created was called: {Message}", notification);

        logger.LogDebug("Send event on message bus for upload picture created");
        var newEvent = new MassPictureUploadedEvent()
        {
            Id = long.Parse(notification.ID),
            Filename = notification.Filename
        };
        await massTransit.Publish<IMassPictureUploadedEvent>(newEvent, cancellationToken);
    }
}