using FamilieLaissMassTransitDefinitions.Contracts.Events;
using MassTransit;
using Upload.API.Mediator.Commands;
using IMediator = MediatR.IMediator;

namespace Upload.API.MassTransit.Consumers;

/// <summary>
/// MassTransit consumer for "PictureConverted"-Event
/// </summary>
public class PictureConvertedConsumer(IMediator mediator, ILogger<PictureConvertedConsumer> logger) : IConsumer<IPictureConvertedEvent>
{
    #region Interface IConsumer
    /// <summary>
    /// Would be called from Masstransit
    /// </summary>
    /// <param name="context">The context for this event</param>
    /// <returns>Task</returns>
    public async Task Consume(ConsumeContext<IPictureConvertedEvent> context)
    {
        //Ausgeben von Logging
        logger.LogInformation("Consumer for Picture-Converted Event was called with {$message}", context.Message);

        //Command zum Ändern der Picture info ausführen
        logger.LogDebug("Execute command with mediator");
        await mediator.Send(new MtrSetPictureStateConvertedCmd() { ID = context.Message.UploadPictureId });
    }
    #endregion
}
