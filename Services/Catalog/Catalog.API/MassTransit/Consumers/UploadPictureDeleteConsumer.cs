using FamilieLaissMassTransitDefinitions.Contracts.Events;
using MassTransit;
using IMediator = MediatR.IMediator;

namespace Catalog.API.MassTransit.Consumers
{
    /// <summary>
    /// MassTransit consumer for "UploadPictureDeleted"-Event
    /// </summary>
    public class UploadPictureDeletedConsumer : IConsumer<iUploadPictureDeletedEvent>
    {
        #region Private Members
        private readonly IMediator _Mediator;
        private readonly ILogger<UploadPictureDeletedConsumer> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="mediator">The Mediator. Will be injected by DI</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public UploadPictureDeletedConsumer(IMediator mediator, ILogger<UploadPictureDeletedConsumer> logger)
        {
            //Übernehmen der Injected Objects
            _Mediator = mediator;
            _Logger = logger;
        }
        #endregion

        #region Interface IConsumer
        /// <summary>
        /// Would be called from Masstransit
        /// </summary>
        /// <param name="context">The context for this event</param>
        /// <returns>Task</returns>
        public async Task Consume(ConsumeContext<iUploadPictureDeletedEvent> context)
        {
            _Logger.LogInformation("Consumer for Upload Picture Deleted was called with message: {@Message}", context.Message);

            _Logger.LogDebug("Sending command for delete upload picture with mediator");
            //var Message = new MtrDeleteUploadPictureCmd(context.Message.ID);
            //await _Mediator.Send(Message);
        }
        #endregion
    }
}
