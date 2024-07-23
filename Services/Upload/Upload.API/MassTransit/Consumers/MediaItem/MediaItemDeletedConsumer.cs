using FamilieLaissMassTransitDefinitions.Contracts.Events.MediaItem;
using FamilieLaissSharedObjects.Enums;
using MassTransit;
using Upload.API.GraphQL.Mutations.UploadPicture;
using Upload.API.GraphQL.Mutations.UploadVideo;
using Upload.API.Mediator.Commands.UploadPicture;
using Upload.API.Mediator.Commands.UploadVideo;
using IMediator = MediatR.IMediator;

namespace Upload.API.MassTransit.Consumers.MediaItem;

/// <summary>
/// MassTransit consumer for "MediaItemDeleted"-Event
/// </summary>
public class MediaItemDeletedConsumer(IMediator mediator, ILogger<MediaItemDeletedConsumer> logger)
    : IConsumer<IMassMediaItemDeletedEvent>
{
    #region Interface IConsumer

    /// <summary>
    /// Would be called from Masstransit
    /// </summary>
    /// <param name="context">The context for this event</param>
    /// <returns>Task</returns>
    public async Task Consume(ConsumeContext<IMassMediaItemDeletedEvent> context)
    {
        logger.LogInformation("Consumer for MediaItemDeleted Event was called with {$message}", context.Message);

        if (context.Message.MediaType == EnumMediaType.Picture)
        {
            if (context.Message.DeleteUploadItem)
            {
                logger.LogDebug("Calling Mediatr command to delete upload picture");
                var newInputData = new DeleteUploadPictureInput()
                {
                    Id = context.Message.UploadItemId,
                };
                var Command = new MtrDeleteUploadPictureCmd() { InputData = newInputData };
                await mediator.Send(Command);
            }
            else
            {
                logger.LogDebug("Calling Mediatr command to unassign upload picture");
                var command = new MtrSetUploadPictureUnAssignedCmd() { InputData = context.Message };
                await mediator.Send(command);
            }
        }
        else
        {
            if (context.Message.DeleteUploadItem)
            {
                logger.LogDebug("Calling Mediatr command to delete upload video");
                var newInputData = new DeleteUploadVideoInput()
                {
                    Id = context.Message.UploadItemId,
                };
                var Command = new MtrDeleteUploadVideoCmd() { InputData = newInputData };
                await mediator.Send(Command);
            }
            else
            {
                logger.LogDebug("Calling Mediatr command to unassign upload video");
                var command = new MtrSetUploadVideoUnAssignedCmd() { InputData = context.Message };
                await mediator.Send(command);
            }
        }
    }

    #endregion
}