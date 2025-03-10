using Catalog.API.Mediator.Commands.UploadPicture;
using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadPicture;
using MassTransit;
using IMediator = MediatR.IMediator;

namespace Catalog.API.MassTransit.Consumers.UploadPicture;

/// <summary>
/// MassTransit consumer for "UploadPictureDeleted"-Event
/// </summary>
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
        logger.LogInformation("Consumer for upload picture deleted event was called with {$message}", context.Message);

        logger.LogDebug("Calling Mediatr command to delete upload picture");
        var command = new MtrDeleteUploadPictureCmd() { Message = context.Message };
        await mediator.Send(command);
    }

    #endregion
}