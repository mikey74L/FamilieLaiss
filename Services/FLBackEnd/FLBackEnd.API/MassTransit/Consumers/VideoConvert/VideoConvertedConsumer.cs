using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FLBackEnd.API.Mediator.Commands.UploadVideo;
using MassTransit;
using MediatR;

namespace FLBackEnd.API.MassTransit.Consumers.VideoConvert;

/// <summary>
/// Mass Transit consumer for "VideoConverted"-Event
/// </summary>
/// <remarks>
/// Primary constructor
/// </remarks>
/// <param name="mediator">Mediatr class. Will be injected by IOC</param>
/// <param name="logger">Logger. Injected by DI</param>
public class VideoConvertedConsumer(IMediator mediator, ILogger<VideoConvertedConsumer> logger)
    : IConsumer<IMassVideoConvertedEvent>
{
    /// <summary>
    /// Would be called from Masstransit
    /// </summary>
    /// <param name="context">The context for this event</param>
    /// <returns>Task</returns>
    public async Task Consume(ConsumeContext<IMassVideoConvertedEvent> context)
    {
        logger.LogInformation("Consumer for Video-Converted Event was called with {$message}", context.Message);

        logger.LogDebug("Execute command with mediator");
        await mediator.Send(new MtrSetVideoStateConvertedCmd()
        {
            UploadVideoId = context.Message.UploadVideoId,
            VideoConvertStatusId = context.Message.ConvertStatusId
        });
    }
}