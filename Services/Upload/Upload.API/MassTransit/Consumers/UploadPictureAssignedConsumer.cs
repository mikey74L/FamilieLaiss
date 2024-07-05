using FamilieLaissMassTransitDefinitions.Contracts.Events;
using MassTransit;
using Upload.API.Mediator.Commands;
using IMediator = MediatR.IMediator;

namespace Upload.API.MassTransit.Consumers;

/// <summary>
/// MassTransit consumer for "UploadPictureAssigned"-Event
/// </summary>
public class UploadPictureAssignedConsumer(IMediator mediator, ILogger<UploadPictureAssignedConsumer> logger) : IConsumer<iUploadPictureAssignedEvent>
{
    #region Interface IConsumer
    /// <summary>
    /// Would be called from Masstransit
    /// </summary>
    /// <param name="context">The context for this event</param>
    /// <returns>Task</returns>
    public async Task Consume(ConsumeContext<iUploadPictureAssignedEvent> context)
    {
        logger.LogInformation("Consumer for UploadPictureAssigned Event was called with {$message}", context.Message);

        logger.LogDebug("Execute command with mediator");
        var command = new MtrSetPictureStateAssignedCmd() { ID = context.Message.ID };
        await mediator.Send(command);
    }
    #endregion
}
