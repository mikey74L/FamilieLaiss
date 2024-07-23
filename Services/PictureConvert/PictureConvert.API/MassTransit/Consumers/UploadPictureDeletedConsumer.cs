using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadPicture;
using MassTransit;
using MediatR;
using PictureConvert.API.Mediator.Commands;

namespace PictureConvert.API.MassTransit.Consumers;

/// <summary>
/// MassTransit consumer for "UploadPictureDeleted"-Event
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="mediator">The Mediator. Will be injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class UploadPictureDeletedConsumer(IMediator mediator, ILogger<UploadPictureDeletedConsumer> logger)
    : IConsumer<IMassUploadPictureDeletedEvent>
{
    #region Interface IConsumer

    /// <summary>
    /// Would be called from Masstransit
    /// </summary>
    /// <param name="context">The context for this event</param>
    /// <returns>Task</returns>
    public async Task Consume(ConsumeContext<IMassUploadPictureDeletedEvent> context)
    {
        //Ausgeben von Logging
        logger.LogInformation("Consumer for Upload Picture Deleted was called: {Message}", context.Message);

        //Aufrufen des Mediator-Commands
        logger.LogDebug("Sending command for delete status entry with mediator");
        var message = new MtrDeleteStatusEntryCmd() { Data = context.Message };
        await mediator.Send(message);
    }

    #endregion
}