using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadVideo;
using MassTransit;
using MediatR;
using Upload.API.Mediator.Commands.UploadVideo;

namespace Upload.API.MassTransit.Consumers.UploadPicture;

/// <summary>
/// MassTransit consumer for "VideoConverted"-Event
/// </summary>
public class VideoConvertedConsumer(IMediator mediator, ILogger<VideoConvertedConsumer> logger)
    : IConsumer<IMassVideoConvertedEvent>
{
    #region Interface IConsumer

    /// <summary>
    /// Would be called from Masstransit
    /// </summary>
    /// <param name="context">The context for this event</param>
    /// <returns>Task</returns>
    public async Task Consume(ConsumeContext<IMassVideoConvertedEvent> context)
    {
        logger.LogInformation("Consumer for video converted event was called with {$message}", context.Message);

        logger.LogDebug("Execute command with mediator");
        await mediator.Send(new MtrSetVideoStateConvertedCmd() { Message = context.Message });
    }

    #endregion
}