using FamilieLaissMassTransitDefinitions.Contracts.Events;
using MassTransit;
using Upload.API.Mediator.Commands;
using IMediator = MediatR.IMediator;

namespace Upload.API.MassTransit.Consumers;

/// <summary>
/// MassTransit consumer for "VideoUploaded"-Event
/// </summary>
public class VideoInfoChangedConsumer(IMediator mediator, ILogger<VideoInfoChangedConsumer> logger) : IConsumer<iVideoInfoChangedEvent>
{
    #region Interface IConsumer
    /// <summary>
    /// Would be called from Masstransit
    /// </summary>
    /// <param name="context">The context for this event</param>
    /// <returns>Task</returns>
    public async Task Consume(ConsumeContext<iVideoInfoChangedEvent> context)
    {
        logger.LogInformation("Consumer for Video-Info-Changed Event was called with {$message}", context.Message);

        logger.LogDebug("Execute command with mediator");
        await mediator.Send(new MtrChangeVideoInfoCmd(context.Message.ID, context.Message.VideoType, context.Message.Height,
            context.Message.Width, context.Message.Duration_Hour, context.Message.Duration_Minute, context.Message.Duration_Second,
            context.Message.GPS_Longitude, context.Message.GPS_Latitude));
    }
    #endregion
}
