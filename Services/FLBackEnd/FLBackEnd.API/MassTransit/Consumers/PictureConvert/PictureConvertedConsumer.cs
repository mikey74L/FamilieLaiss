using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FLBackEnd.API.Mediator.Commands.UploadPicture;
using MassTransit;
using MediatR;

namespace FLBackEnd.API.MassTransit.Consumers.PictureConvert;

/// <summary>
/// MassTransit's consumer for "PictureConverted"-Event
/// </summary>
public class PictureConvertedConsumer(IMediator mediator, ILogger<PictureConvertedConsumer> logger)
    : IConsumer<IPictureConvertedEvent>
{
    /// <summary>
    /// Would be called from Masstransit
    /// </summary>
    /// <param name="context">The context for this event</param>
    /// <returns>Task</returns>
    public async Task Consume(ConsumeContext<IPictureConvertedEvent> context)
    {
        logger.LogInformation("Consumer for Picture-Converted Event was called with {$message}", context.Message);

        logger.LogDebug("Execute command with mediator");
        await mediator.Send(new MtrSetPictureStateConvertedCmd()
        {
            UploadPictureId = context.Message.UploadPictureId, PictureConvertStatusId = context.Message.ConvertStatusId
        });
    }
}