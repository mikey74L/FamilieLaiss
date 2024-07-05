using FamilieLaissMassTransitDefinitions.Contracts.Events;
using MassTransit;
using Upload.API.Mediator.Commands;
using IMediator = MediatR.IMediator;

namespace Upload.API.MassTransit.Consumers;

/// <summary>
/// MassTransit consumer for "PictureUploaded"-Event
/// </summary>
public class PictureInfoChangedConsumer(IMediator mediator, ILogger<PictureInfoChangedConsumer> logger) : IConsumer<iPictureInfoChangedEvent>
{
    #region Interface IConsumer
    /// <summary>
    /// Would be called from Masstransit
    /// </summary>
    /// <param name="context">The context for this event</param>
    /// <returns>Task</returns>
    public async Task Consume(ConsumeContext<iPictureInfoChangedEvent> context)
    {
        logger.LogInformation("Consumer for Picture-Info-Changed Event was called with {$message}:", context.Message);

        logger.LogDebug("Execute command with mediator");
        await mediator.Send(new MtrChangePictureInfoCmd(context.Message.ID, context.Message.Height, context.Message.Width));
    }
    #endregion
}
