using FamilieLaissMassTransitDefinitions.Contracts.Events;
using MassTransit;
using Upload.API.Mediator.Commands;
using IMediator = MediatR.IMediator;

namespace Upload.API.MassTransit.Consumers;

/// <summary>
/// MassTransit consumer for "VideoConverted"-Event
/// </summary>
public class VideoConvertedConsumer(IMediator mediator, ILogger<VideoConvertedConsumer> logger) : IConsumer<IVideoConvertedEvent>
{
    #region Interface IConsumer
    /// <summary>
    /// Would be called from Masstransit
    /// </summary>
    /// <param name="context">The context for this event</param>
    /// <returns>Task</returns>
    public async Task Consume(ConsumeContext<IVideoConvertedEvent> context)
    {
        logger.LogInformation("Consumer for Video-Converted Event was called with {$message}", context.Message);

        logger.LogDebug("Execute command with mediator");
        await mediator.Send(new MtrSetVideoStateConvertedCmd() { ID = context.Message.UploadVideoId });
    }
    #endregion
}
