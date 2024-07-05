using FamilieLaissMassTransitDefinitions.Contracts.Events;
using MassTransit;
using IMediator = MediatR.IMediator;

namespace Catalog.API.MassTransit.Consumers
{
    /// <summary>
    /// MassTransit consumer for "UploadVideoDeleted"-Event
    /// </summary>
    public class UploadVideoDeletedConsumer : IConsumer<iUploadVideoDeletedEvent>
    {
        #region Private Members
        private readonly IMediator _Mediator;
        private readonly ILogger<UploadVideoDeletedConsumer> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="mediator">The Mediator. Will be injected by DI</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public UploadVideoDeletedConsumer(IMediator mediator, ILogger<UploadVideoDeletedConsumer> logger)
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
        public async Task Consume(ConsumeContext<iUploadVideoDeletedEvent> context)
        {
            _Logger.LogInformation("Consumer for Upload Video Deleted was called with message: {@Message}", context.Message);

            _Logger.LogDebug("Sending command for delete upload video entry with mediator");
            //var Message = new MtrDeleteUploadVideoCmd(context.Message.ID);
            //await _Mediator.Send(Message);
        }
        #endregion
    }
}
