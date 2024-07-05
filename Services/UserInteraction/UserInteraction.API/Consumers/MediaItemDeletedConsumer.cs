using FamilieLaissMassTransitDefinitions.Contracts.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using UserInteraction.API.Commands;
using IMediator = MediatR.IMediator;

namespace UserInteractions.API.Consumers
{
    /// <summary>
    /// MassTransit consumer for "MediaItemDeleted"-Event
    /// </summary>
    public class MediaItemDeletedConsumer : IConsumer<iMediaItemDeletedEvent>
    {
        #region Private Members
        private readonly IMediator _Mediator;
        private readonly ILogger<MediaItemDeletedConsumer> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="mediator">The mediator. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MediaItemDeletedConsumer(IMediator mediator, ILogger<MediaItemDeletedConsumer> logger)
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
        public async Task Consume(ConsumeContext<iMediaItemDeletedEvent> context)
        {
            //Ausgeben von Logging
            _Logger.LogInformation("Consumer for media item deleted was called with following parameters:");
            _Logger.LogDebug($"ID          : {context.Message.ID}");
            _Logger.LogDebug($"IsPicture   : {context.Message.IsPicture}");
            _Logger.LogDebug($"UploadItemID: {context.Message.UploadItemID}");

            //Aufrufen des Mediator-Commands
            _Logger.LogDebug("Sending command for delete user interaction info with mediator");
            var Message = new MtrDeleteUserInteractionInfoCmd(context.Message.ID);
            await _Mediator.Send(Message);
        }
        #endregion
    }
}
