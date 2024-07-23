using FamilieLaissMassTransitDefinitions.Contracts.Commands.UploadPicture;
using MassTransit;
using MediatR;
using Upload.API.Mediator.Commands.UploadPicture;

namespace Upload.API.MassTransit.Consumers.UploadPicture;

/// <summary>
/// MassTransit consumer for command set upload picture dimensions
/// </summary>
public class SetUploadPictureDimensionsConsumer(IMediator mediator, ILogger<SetUploadPictureDimensionsConsumer> logger)
    : IConsumer<IMassSetUploadPictureDimensionsCmd>
{
    #region Interface IConsumer

    /// <summary>
    /// Would be called from Masstransit
    /// </summary>
    /// <param name="context">The context for this event</param>
    /// <returns>Task</returns>
    public async Task Consume(ConsumeContext<IMassSetUploadPictureDimensionsCmd> context)
    {
        logger.LogInformation("Consumer for set upload picture dimensions command was called with {$message}",
            context.Message);

        logger.LogDebug("Calling Mediatr command to set dimensions for upload picture");
        var command = new MtrSetUploadPictureDimensionsCmd() { Message = context.Message };
        await mediator.Send(command);
    }

    #endregion
}