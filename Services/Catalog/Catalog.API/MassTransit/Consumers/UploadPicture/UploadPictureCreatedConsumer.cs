using Catalog.API.Mediator.Commands.UploadPicture;
using FamilieLaissMassTransitDefinitions.Contracts.Events;
using MassTransit;
using IMediator = MediatR.IMediator;

namespace Catalog.API.MassTransit.Consumers.UploadPicture;

/// <summary>
/// MassTransit consumer for "UploadPictureCreated"-Event
/// </summary>
public class UploadPictureCreatedConsumer(IMediator mediator, ILogger<UploadPictureCreatedConsumer> logger)
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
        logger.LogInformation("Consumer for upload picture created event was called with {$message}", context.Message);

        logger.LogDebug("Calling Mediatr command to add upload picture");
        var command = new MtrAddUploadPictureCmd() { Message = context.Message };
        await mediator.Send(command);
    }

    #endregion
}