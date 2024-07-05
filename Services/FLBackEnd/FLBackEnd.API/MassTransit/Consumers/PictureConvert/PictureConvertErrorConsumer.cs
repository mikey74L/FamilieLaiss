using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FLBackEnd.API.Mediator.Commands.PictureConvertStatus;
using MassTransit;
using MediatR;

namespace FLBackEnd.API.MassTransit.Consumers.PictureConvert;

/// <summary>
/// MassTransit's consumer for "PictureConvertErrorConsumer"-Event
/// </summary>
public class PictureConvertErrorConsumer(
    IMediator mediator,
    ILogger<PictureConvertErrorConsumer> logger)
    : IConsumer<IPictureConvertErrorEvent>
{
    /// <summary>
    /// Would be called from Masstransit
    /// </summary>
    /// <param name="context">The context for this event</param>
    /// <returns>Task</returns>
    public async Task Consume(ConsumeContext<IPictureConvertErrorEvent> context)
    {
        logger.LogInformation("Consumer for Picture-Convert-Error Event was called with {$message}",
            context.Message);

        logger.LogDebug("Execute command with mediator");
        await mediator.Send(new MtrPictureConvertStatusErrorCmd()
        {
            UploadPictureId = context.Message.UploadPictureId,
            PictureConvertStatusId = context.Message.ConvertStatusId
        });
    }
}