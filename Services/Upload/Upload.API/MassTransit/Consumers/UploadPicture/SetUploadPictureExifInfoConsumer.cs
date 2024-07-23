using FamilieLaissMassTransitDefinitions.Contracts.Commands.UploadPicture;
using MassTransit;
using Upload.API.Mediator.Commands.UploadPicture;
using IMediator = MediatR.IMediator;

namespace Upload.API.MassTransit.Consumers.UploadPicture;

/// <summary>
/// MassTransit consumer for upload picture set exif info  command
/// </summary>
public class SetUploadPictureExifInfoConsumer(IMediator mediator, ILogger<SetUploadPictureExifInfoConsumer> logger)
    : IConsumer<IMassSetUploadPictureExifInfoCmd>
{
    #region Interface IConsumer

    /// <summary>
    /// Would be called from Masstransit
    /// </summary>
    /// <param name="context">The context for this event</param>
    /// <returns>Task</returns>
    public async Task Consume(ConsumeContext<IMassSetUploadPictureExifInfoCmd> context)
    {
        logger.LogInformation("Consumer for set upload picture exif info command was called with {$message}",
            context.Message);

        logger.LogDebug("Calling Mediatr command to set exif info for upload picture");
        var command = new MtrUploadPictureSetExifInfoCmd() { Message = context.Message };
        await mediator.Send(command);
    }

    #endregion
}