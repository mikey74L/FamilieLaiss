using FamilieLaissMassTransitDefinitions.Contracts.Events;
using MassTransit;

namespace Catalog.API.MassTransit.Consumers
{
    /// <summary>
    /// MassTransit consumer for "PictureUploaded"-Event
    /// </summary>
    public class PictureUploadedConsumer : IConsumer<iPictureUploadedEvent>
    {
        #region Private Members
        private readonly MediatR.IMediator _Mediator;
        private readonly ILogger<PictureUploadedConsumer> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="mediator">The mediator. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public PictureUploadedConsumer(MediatR.IMediator mediator, ILogger<PictureUploadedConsumer> logger)
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
        public async Task Consume(ConsumeContext<iPictureUploadedEvent> context)
        {
            _Logger.LogInformation("Consumer for picture uploaded was called with message: {@Message}", context.Message);

            //var Message = new MtrCreateUploadPictureCmd(context.Message.ID, context.Message.Filename);
            //await _Mediator.Send(Message);
        }
        #endregion
    }
}
