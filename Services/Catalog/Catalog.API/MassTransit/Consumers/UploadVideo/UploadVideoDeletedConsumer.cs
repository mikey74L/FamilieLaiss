using Catalog.API.Mediator.Commands.UploadVideo;
using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadVideo;
using MassTransit;
using IMediator = MediatR.IMediator;

namespace Catalog.API.MassTransit.Consumers.UploadVideo;

/// <summary>
/// MassTransit consumer for "UploadVideoDeleted"-Event
/// </summary>
public class UploadVideoDeletedConsumer(IMediator mediator, ILogger<UploadVideoDeletedConsumer> logger)
    : IConsumer<IMassUploadVideoDeletedEvent>
{
    #region Interface IConsumer

    /// <summary>
    /// Would be called from Masstransit
    /// </summary>
    /// <param name="context">The context for this event</param>
    /// <returns>Task</returns>
    public async Task Consume(ConsumeContext<IMassUploadVideoDeletedEvent> context)
    {
        logger.LogInformation("Consumer for upload video deleted event was called with {$message}", context.Message);

        logger.LogDebug("Calling Mediatr command to delete upload video");
        var command = new MtrDeleteUploadVideoCmd() { Message = context.Message };
        await mediator.Send(command);
    }

    #endregion
}