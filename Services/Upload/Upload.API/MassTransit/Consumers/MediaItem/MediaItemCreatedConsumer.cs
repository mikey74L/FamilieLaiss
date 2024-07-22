using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FamilieLaissSharedObjects.Enums;
using MassTransit;
using MediatR;
using Upload.API.Mediator.Commands.UploadPicture;
using Upload.API.Mediator.Commands.UploadVideo;

namespace Upload.API.MassTransit.Consumers.MediaItem;

/// <summary>
/// MassTransit consumer for "MediaItemCreated"-Event
/// </summary>
public class MediaItemCreatedConsumer(IMediator mediator, ILogger<MediaItemCreatedConsumer> logger)
    : IConsumer<IMassMediaItemCreatedEvent>
{
    #region Interface IConsumer

    /// <summary>
    /// Would be called from Masstransit
    /// </summary>
    /// <param name="context">The context for this event</param>
    /// <returns>Task</returns>
    public async Task Consume(ConsumeContext<IMassMediaItemCreatedEvent> context)
    {
        logger.LogInformation("Consumer for media item created event was called with {$message}", context.Message);

        if (context.Message.MediaType == EnumMediaType.Picture)
        {
            logger.LogDebug("Calling Mediatr command to assign upload picture");
            var command = new MtrSetUploadPictureAssignedCmd() { InputData = context.Message };
            await mediator.Send(command);
        }
        else
        {
            logger.LogDebug("Calling Mediatr command to assign upload picture");
            var command = new MtrSetUploadVideoAssignedCmd() { InputData = context.Message };
            await mediator.Send(command);
        }
    }

    #endregion
}