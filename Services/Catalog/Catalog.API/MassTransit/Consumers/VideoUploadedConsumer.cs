using FamilieLaissMassTransitDefinitions.Contracts.Events;
using MassTransit;

namespace Catalog.API.MassTransit.Consumers
{
    /// <summary>
    /// MassTransit consumer for "VideoUploaded"-Event
    /// </summary>
    public class VideoUploadedConsumer : IConsumer<iVideoUploadedEvent>
    {
        #region Private Members
        private readonly MediatR.IMediator _Mediator;
        private readonly ILogger<VideoUploadedConsumer> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="mediator">The mediator. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public VideoUploadedConsumer(MediatR.IMediator mediator, ILogger<VideoUploadedConsumer> logger)
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
        public async Task Consume(ConsumeContext<iVideoUploadedEvent> context)
        {
            _Logger.LogInformation("Consumer for video uploaded was called with message: {@Message}", context.Message);

            _Logger.LogDebug("Sending command for create upload video item with mediator");
            //var Message = new MtrCreateUploadVideoCmd(context.Message.ID, context.Message.Filename);
            //await _Mediator.Send(Message);
        }
        #endregion
    }
}
