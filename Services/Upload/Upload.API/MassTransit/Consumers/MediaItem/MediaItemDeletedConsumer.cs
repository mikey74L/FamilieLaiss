using FamilieLaissMassTransitDefinitions.Contracts.Events;
using MassTransit;
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

        if (context.Message.IsPicture)
        {
            if (context.Message.DeleteUploadItem)
            {
                logger.LogDebug("Calling Mediatr command to delete upload picture");
                //TODO
                //var Command = new MtrDeleteUploadPictureCmd(context.Message.UploadItemID);
                //await _Mediator.Send(Command);
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
                //TODO
                //var Command = new MtrDeleteUploadVideoCmd(context.Message.UploadItemID);
                //await _Mediator.Send(Command);
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