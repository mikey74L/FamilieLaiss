using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadPicture;
using FamilieLaissMassTransitDefinitions.Events.UploadPicture;
using MassTransit;
using MediatR;
using Upload.Domain.DomainEvents.UploadPicture;

namespace Upload.API.DomainEvents.UploadPicture;

public class DomainEventUploadPictureDeletedHandler(
    ILogger<DomainEventUploadPictureDeletedHandler> logger,
    IBus massTransit) : INotificationHandler<DomainEventUploadPictureDeleted>
{
    public async Task Handle(DomainEventUploadPictureDeleted notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Mediatr-Notification-Handler for domain event upload picture deleted was called: {Message}", notification);

        logger.LogDebug("Send event on message bus for upload picture deleted");
        var newEvent = new MassUploadPictureDeletedEvent()
        {
            Id = long.Parse(notification.ID),
        };
        await massTransit.Publish<IMassUploadPictureDeletedEvent>(newEvent, cancellationToken);
    }
}