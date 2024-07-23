using Catalog.API.Mediator.Commands.UploadVideo;
using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadVideo;
using MassTransit;
using IMediator = MediatR.IMediator;

namespace Catalog.API.MassTransit.Consumers.UploadVideo;

/// <summary>
/// MassTransit consumer for "UploadVideoCreated"-Event
/// </summary>
public class UploadVideoCreatedConsumer(IMediator mediator, ILogger<UploadVideoCreatedConsumer> logger)
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
        logger.LogInformation("Consumer for upload video created event was called with {$message}", context.Message);

        logger.LogDebug("Calling Mediatr command to add upload video");
        var command = new MtrAddUploadVideoCmd() { Message = context.Message };
        await mediator.Send(command);
    }

    #endregion
}