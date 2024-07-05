using FamilieLaissMassTransitDefinitions.Contracts.Events;
using MassTransit;
using Upload.API.Mediator.Commands;
using IMediator = MediatR.IMediator;

namespace Upload.API.MassTransit.Consumers;

/// <summary>
/// MassTransit consumer for "UploadVideoAssigned"-Event
/// </summary>
public class UploadVideoAssignedConsumer(IMediator mediator, ILogger<UploadVideoAssignedConsumer> logger) : IConsumer<iUploadVideoAssignedEvent>
{
    #region Interface IConsumer
    /// <summary>
    /// Would be called from Masstransit
    /// </summary>
    /// <param name="context">The context for this event</param>
    /// <returns>Task</returns>
    public async Task Consume(ConsumeContext<iUploadVideoAssignedEvent> context)
    {
        logger.LogInformation("Consumer for UploadVideoAssigned Event was called with {$message}", context.Message);

        logger.LogDebug("Execute command with mediator");
        var command = new MtrSetVideoStateAssignedCmd() { ID = context.Message.ID };
        await mediator.Send(command);
    }
    #endregion
}
