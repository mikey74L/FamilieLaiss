using Catalog.Domain.DomainEvents;
using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FamilieLaissMassTransitDefinitions.Events;
using MassTransit;
using MediatR;

namespace Catalog.API.Mediator.EventHandler.MediaItem
{
    /// <summary>
    /// Event handler for upload video assigned
    /// </summary>
    public class MtrEventUploadVideoAssignedHandler : INotificationHandler<MtrEventUploadVideoAssigned>
    {
        #region Private Members
        private readonly IBus _ServiceBus;
        private readonly ILogger<MtrEventUploadVideoAssignedHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="serviceBus">Masstransit service bus. Will be injected by DI-Container</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrEventUploadVideoAssignedHandler(IBus serviceBus, ILogger<MtrEventUploadVideoAssignedHandler> logger)
        {
            _ServiceBus = serviceBus;
            _Logger = logger;
        }
        #endregion

        #region Mediatr-Handler
        /// <summary>
        /// Will be called by Mediatr
        /// </summary>
        /// <param name="notification">The notification data</param>
        /// <param name="cancellationToken">The cancelation token</param>
        /// <returns>Task</returns>
        public async Task Handle(MtrEventUploadVideoAssigned notification, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation($"Mediatr-Handler for upload video assigned was called for UploadItemID = {notification.ID}");

            //Versenden des Events über den Service Bus
            _Logger.LogDebug("Sending event over service bus");
            var Event = new UploadVideoAssignedEvent(Convert.ToInt64(notification.ID));
            await _ServiceBus.Publish<iUploadVideoAssignedEvent>(Event);
        }
        #endregion
    }
}
