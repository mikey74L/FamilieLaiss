using FamilieLaissMassTransitDefinitions.Contracts.Events;
using MassTransit;
using VideoConvert.API.Mediator.Commands;

namespace VideoConvert.API.MassTransit.Consumers;

/// <summary>
/// MassTransit consumer for "VideoUploaded"-Event
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="mediator">The mediator. Will be injected by DI.</param>
/// <param name="logger">Logger. Injected by DI</param>
public class VideoUploadedConsumer(MediatR.IMediator mediator, ILogger<VideoUploadedConsumer> logger)
    : IConsumer<IMassVideoUploadedEvent>
{
    #region Interface IConsumer

    /// <summary>
    /// Would be called from Masstransit
    /// </summary>
    /// <param name="context">The context for this event</param>
    /// <returns>Task</returns>
    public async Task Consume(ConsumeContext<IMassVideoUploadedEvent> context)
    {
        logger.LogInformation("Consumer for video uploaded was called: {Message}", context.Message);

        logger.LogDebug("Sending command for make status entry with mediator");
        var message = new MtrMakeStatusEntryCmd() { Data = context.Message };
        await mediator.Send(message);
    }
}

#endregion