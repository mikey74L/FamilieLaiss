using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FLBackEnd.API.Mediator.Commands.VideoConvertStatus;
using MassTransit;
using MediatR;

namespace FLBackEnd.API.MassTransit.Consumers.VideoConvert;

/// <summary>
/// MassTransit's consumer for "VideoConvertProgress"-Event
/// </summary>
public class VideoConvertProgressConsumer(
    IMediator mediator,
    ILogger<VideoConvertProgressConsumer> logger)
    : IConsumer<IVideoConvertProgressEvent>
{
    /// <summary>
    /// Would be called from Masstransit
    /// </summary>
    /// <param name="context">The context for this event</param>
    /// <returns>Task</returns>
    public async Task Consume(ConsumeContext<IVideoConvertProgressEvent> context)
    {
        logger.LogInformation("Consumer for Video-Convert-Progress Event was called with {$message}",
            context.Message);

        logger.LogDebug("Execute command with mediator");
        await mediator.Send(new MtrVideoConvertStatusProgressCmd()
        {
            UploadVideoId = context.Message.UploadVideoId,
            VideoConvertStatusId = context.Message.ConvertStatusId
        });
    }
}