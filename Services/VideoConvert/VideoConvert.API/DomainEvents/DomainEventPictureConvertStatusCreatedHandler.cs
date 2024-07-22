using FamilieLaissMassTransitDefinitions.Commands;
using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using MassTransit;
using MediatR;
using VideoConvert.Domain.DomainEvents;

namespace VideoConvert.API.DomainEvents;

public class DomainEventVideoConvertStatusCreatedHandler(
    ILogger<DomainEventVideoConvertStatusCreatedHandler> logger,
    IBus massTransit) : INotificationHandler<DomainEventVideoConvertStatusCreated>
{
    public async Task Handle(DomainEventVideoConvertStatusCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Mediatr-Notification-Handler for domain event video convert status created was called: {Message}",
            notification);

        logger.LogDebug("Send event on message bus for start conversion");
        var newCommand = new MassConvertVideoCmd()
        {
            Id = notification.UploadVideoId,
            ConvertStatusId = long.Parse(notification.ID),
            OriginalName = notification.OriginalFilename
        };
        await massTransit.Send<IMassConvertVideoCmd>(newCommand, cancellationToken);
    }
}