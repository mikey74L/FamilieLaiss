using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FLBackEnd.API.Mediator.Commands.PictureConvertStatus;
using MassTransit;
using MediatR;

namespace FLBackEnd.API.MassTransit.Consumers.PictureConvert;

/// <summary>
/// MassTransit's consumer for "PictureConvertProgress"-Event
/// </summary>
public class PictureConvertProgressConsumer(
    IMediator mediator,
    ILogger<PictureConvertProgressConsumer> logger)
    : IConsumer<IMassPictureConvertProgressEvent>
{
    /// <summary>
    /// Would be called from Masstransit
    /// </summary>
    /// <param name="context">The context for this event</param>
    /// <returns>Task</returns>
    public async Task Consume(ConsumeContext<IMassPictureConvertProgressEvent> context)
    {
        logger.LogInformation("Consumer for Picture-Convert-Progress Event was called with {$message}",
            context.Message);

        logger.LogDebug("Execute command with mediator");
        await mediator.Send(new MtrPictureConvertStatusProgressCmd()
        {
            UploadPictureId = context.Message.UploadPictureId,
            PictureConvertStatusId = context.Message.ConvertStatusId
        });
    }
}