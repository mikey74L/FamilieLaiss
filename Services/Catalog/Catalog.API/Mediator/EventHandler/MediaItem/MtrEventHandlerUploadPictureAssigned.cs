using Catalog.Domain.DomainEvents;
using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FamilieLaissMassTransitDefinitions.Events;
using MassTransit;
using MediatR;

namespace Catalog.API.Mediator.EventHandler.MediaItem
{
    /// <summary>
    /// Event handler for upload picture assigned
    /// </summary>
    public class MtrEventUploadPictureAssignedHandler : INotificationHandler<MtrEventUploadPictureAssigned>
    {
        #region Private Members
        private readonly IBus _ServiceBus;
        private readonly ILogger<MtrEventUploadPictureAssignedHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="serviceBus">Masstransit service bus. Will be injected by DI-Container</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrEventUploadPictureAssignedHandler(IBus serviceBus, ILogger<MtrEventUploadPictureAssignedHandler> logger)
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
        public async Task Handle(MtrEventUploadPictureAssigned notification, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation($"Mediatr-Handler for upload picture assigned was called for UploadItemID = {notification.ID}");

            //Versenden des Events über den Service Bus
            _Logger.LogDebug("Sending event over service bus");
            var Event = new UploadPictureAssignedEvent(Convert.ToInt64(notification.ID));
            await _ServiceBus.Publish<iUploadPictureAssignedEvent>(Event);
        }
        #endregion
    }
}
