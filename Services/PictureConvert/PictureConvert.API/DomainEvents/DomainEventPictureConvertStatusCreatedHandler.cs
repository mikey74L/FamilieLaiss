using FamilieLaissMassTransitDefinitions.Commands.UploadPicture;
using FamilieLaissMassTransitDefinitions.Contracts.Commands.UploadPicture;
using MassTransit;
using MediatR;
using PictureConvert.Domain.DomainEvents;

namespace PictureConvert.API.DomainEvents;

public class DomainEventPictureConvertStatusCreatedHandler(
    ILogger<DomainEventPictureConvertStatusCreatedHandler> logger,
    IBus massTransit) : INotificationHandler<DomainEventPictureConvertStatusCreated>
{
    public async Task Handle(DomainEventPictureConvertStatusCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Mediatr-Notification-Handler for domain event picture convert status created was called: {Message}",
            notification);

        logger.LogDebug("Send event on message bus for start conversion");
        var newCommand = new MassConvertPictureCmd()
        {
            Id = notification.UploadPictureId,
            ConvertStatusId = long.Parse(notification.ID),
            OriginalName = notification.OriginalFilename
        };
        await massTransit.Send<IMassConvertPictureCmd>(newCommand, cancellationToken);
    }
}