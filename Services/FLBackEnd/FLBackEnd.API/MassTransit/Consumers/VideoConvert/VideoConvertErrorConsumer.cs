using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FLBackEnd.API.Mediator.Commands.VideoConvertStatus;
using MassTransit;
using MediatR;

namespace FLBackEnd.API.MassTransit.Consumers.VideoConvert;

/// <summary>
/// MassTransit's consumer for "VideoConvertErrorConsumer"-Event
/// </summary>
public class VideoConvertErrorConsumer(
    IMediator mediator,
    ILogger<VideoConvertErrorConsumer> logger)
    : IConsumer<IMassVideoConvertErrorEvent>
{
    /// <summary>
    /// Would be called from Masstransit
    /// </summary>
    /// <param name="context">The context for this event</param>
    /// <returns>Task</returns>
    public async Task Consume(ConsumeContext<IMassVideoConvertErrorEvent> context)
    {
        logger.LogInformation("Consumer for Video-Convert-Error Event was called with {$message}",
            context.Message);

        logger.LogDebug("Execute command with mediator");
        await mediator.Send(new MtrVideoConvertStatusErrorCmd()
        {
            UploadVideoId = context.Message.UploadVideoId,
            VideoConvertStatusId = context.Message.ConvertStatusId
        });
    }
}