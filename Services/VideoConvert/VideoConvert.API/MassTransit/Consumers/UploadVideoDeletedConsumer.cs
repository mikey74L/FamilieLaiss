using FamilieLaissMassTransitDefinitions.Contracts.Events;
using MassTransit;
using VideoConvert.API.Mediator.Commands;
using IMediator = MediatR.IMediator;

namespace VideoConvert.API.MassTransit.Consumers;

/// <summary>
/// MassTransit consumer for "UploadVideoDeleted"-Event
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="mediator">The Mediator. Will be injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
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
        //Ausgeben von Logging
        logger.LogInformation("Consumer for upload video deleted was called: {Message}", context.Message);

        //Aufrufen des Mediator-Commands
        logger.LogDebug("Sending command for delete status entry with mediator");
        var message = new MtrDeleteStatusEntryCmd() { Data = context.Message };
        await mediator.Send(message);
    }

    #endregion
}