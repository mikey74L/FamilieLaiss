using FamilieLaissMassTransitDefinitions.Contracts.Events;
using MassTransit;
using MediatR;
using Upload.API.Mediator.Commands.UploadPicture;

namespace Upload.API.MassTransit.Consumers.UploadPicture;

/// <summary>
/// MassTransit consumer for "PictureConverted"-Event
/// </summary>
public class PictureConvertedConsumer(IMediator mediator, ILogger<PictureConvertedConsumer> logger)
    : IConsumer<IMassPictureConvertedEvent>
{
    #region Interface IConsumer

    /// <summary>
    /// Would be called from Masstransit
    /// </summary>
    /// <param name="context">The context for this event</param>
    /// <returns>Task</returns>
    public async Task Consume(ConsumeContext<IMassPictureConvertedEvent> context)
    {
        //Ausgeben von Logging
        logger.LogInformation("Consumer for Picture-Converted Event was called with {$message}", context.Message);

        //Command zum Ändern der Picture info ausführen
        logger.LogDebug("Execute command with mediator");
        await mediator.Send(new MtrSetPictureStateConvertedCmd() { Message = context.Message });
    }

    #endregion
}