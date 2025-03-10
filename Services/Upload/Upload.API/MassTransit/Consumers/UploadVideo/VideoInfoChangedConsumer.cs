using FamilieLaissMassTransitDefinitions.Contracts.Commands.UploadVideo;
using MassTransit;
using MediatR;
using Upload.API.Mediator.Commands.UploadVideo;

namespace Upload.API.MassTransit.Consumers.UploadVideo;

/// <summary>
/// MassTransit consumer for set video info command
/// </summary>
public class VideoInfoChangedConsumer(IMediator mediator, ILogger<VideoInfoChangedConsumer> logger)
    : IConsumer<IMassSetVideoInfoDataCmd>
{
    #region Interface IConsumer

    /// <summary>
    /// Would be called from Masstransit
    /// </summary>
    /// <param name="context">The context for this command</param>
    /// <returns>Task</returns>
    public async Task Consume(ConsumeContext<IMassSetVideoInfoDataCmd> context)
    {
        logger.LogInformation("Consumer for set video info command was called with {$message}", context.Message);

        logger.LogDebug("Execute command with mediator");
        await mediator.Send(new MtrSetVideoInfoCmd() { Message = context.Message });
    }

    #endregion
}