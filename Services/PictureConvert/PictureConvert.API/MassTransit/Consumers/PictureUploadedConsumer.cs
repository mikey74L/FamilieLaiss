using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadPicture;
using MassTransit;
using PictureConvert.API.Mediator.Commands;

namespace PictureConvert.API.MassTransit.Consumers;

/// <summary>
/// MassTransit consumer for "PictureUploaded"-Event
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="mediator">The mediator. Will be injected by DI.</param>
/// <param name="logger">Logger. Injected by DI</param>
public class PictureUploadedConsumer(MediatR.IMediator mediator, ILogger<PictureUploadedConsumer> logger)
    : IConsumer<IMassPictureUploadedEvent>
{
    #region Interface IConsumer

    /// <summary>
    /// Would be called from Masstransit
    /// </summary>
    /// <param name="context">The context for this event</param>
    /// <returns>Task</returns>
    public async Task Consume(ConsumeContext<IMassPictureUploadedEvent> context)
    {
        logger.LogInformation("Consumer for picture uploaded was called: {Message}", context.Message);

        logger.LogDebug("Sending command for make status entry with mediator");
        var message = new MtrMakeStatusEntryCmd() { Data = context.Message };
        await mediator.Send(message);
    }

    #endregion
}