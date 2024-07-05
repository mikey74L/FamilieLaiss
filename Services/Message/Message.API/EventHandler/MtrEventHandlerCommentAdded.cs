using FamilieLaissSharedObjects.Enums;
using MediatR;
using Message.API.Hubs;
using Message.Domain.DomainEvents;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Message.API.EventHandler
{
    /// <summary>
    /// Event handler for message for user created
    /// </summary>
    public class MtrEventHandlerMessageForUserCreated : INotificationHandler<MtrEventMessageForUserCreated>
    {
        #region Private Members
        private readonly IHubContext<MessageHub> _HubContext;
        private readonly ILogger<MtrEventHandlerMessageForUserCreated> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">SignalR-Hub for messages. Will be injected by DI-Container</param>
        /// <param name="hubContext">Logger. Injected by DI</param>
        public MtrEventHandlerMessageForUserCreated(ILogger<MtrEventHandlerMessageForUserCreated> logger)
        {
            //_HubContext = hubContext;
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
        public async Task Handle(MtrEventMessageForUserCreated notification, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for message for user created was called with following parameters:");
            _Logger.LogDebug($"User-Name      : {notification.UserName.Substring(1, 3)}");
            _Logger.LogDebug($"Prio           : {notification.Prio}");
            _Logger.LogDebug($"MessageID      : {notification.ID}");
            _Logger.LogDebug($"Text-German    : {notification.Text_German}");
            _Logger.LogDebug($"Text-English   : {notification.Text_English}");
            _Logger.LogDebug($"Additional-Data: {notification.AdditionalData}");
            _Logger.LogDebug($"Create-Date    : {notification.CreateDate}");

            //SignalR verwenden um die neue Nachricht zu versenden
            _Logger.LogDebug("Send message with SignalR to client");
            switch (notification.Prio)
            {
                case enMessagePrio.Error:
                    await _HubContext.Clients.User(notification.UserName).SendAsync("NewMessageError", notification.ID, notification.Text_German, notification.Text_English, 
                        notification.CreateDate, notification.AdditionalData);
                    break;
                case enMessagePrio.Warning:
                    await _HubContext.Clients.User(notification.UserName).SendAsync("NewMessageWarning", notification.ID, notification.Text_German, notification.Text_English, 
                        notification.CreateDate, notification.AdditionalData);
                    break;
                case enMessagePrio.Info:
                    await _HubContext.Clients.User(notification.UserName).SendAsync("NewMessageInfo", notification.ID, notification.Text_German, notification.Text_English,
                        notification.CreateDate, notification.AdditionalData);
                    break;
            }
        }
        #endregion
    }
}
