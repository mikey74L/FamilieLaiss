using Catalog.Domain.DomainEvents;
using MassTransit;
using MediatR;

namespace Catalog.API.Mediator.EventHandler.MediaGroup
{
    /// <summary>
    /// Event handler for media group created
    /// </summary>
    public class MtrEventMediaGroupCreatedHandler : INotificationHandler<MtrEventMediaGroupCreated>
    {
        #region Private Members
        private readonly IBus _ServiceBus;
        private readonly ILogger<MtrEventMediaGroupCreatedHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="serviceBus">Masstransit service bus. Will be injected by DI-Container</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrEventMediaGroupCreatedHandler(IBus serviceBus, ILogger<MtrEventMediaGroupCreatedHandler> logger)
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
        public async Task Handle(MtrEventMediaGroupCreated notification, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for media item created was called for {Notification}", notification);

            //Command für neue Message zusammenstellen
            //string GermanText = notification.MediaType == EnumMediaType.Picture ? 
            //    string.Format(MessageText.NewAlbum_Photo_Created_German, notification.GermanName) : string.Format(MessageText.NewAlbum_Video_Created_German, notification.GermanName);
            //string EnglishText = notification.MediaType == EnumMediaType.Picture ? 
            //    string.Format(MessageText.NewAlbum_Photo_Created_English, notification.EnglishName) : string.Format(MessageText.NewAlbum_Video_Created_English, notification.EnglishName);
            //var CmdOne = new CreateMessageForUserGroupCmd(enMessagePrio.Info, UserRoleConstants.User, GermanText, EnglishText, "");
            //var CmdTwo = new CreateMessageForUserGroupCmd(enMessagePrio.Info, UserRoleConstants.FamilyUser, GermanText, EnglishText, "");

            //Versenden des Commands über den Service Bus
            _Logger.LogDebug("Sending Command over service bus");
            //await _ServiceBus.Send<iCreateMessageForUserGroupCmd>(CmdOne);
            //await _ServiceBus.Send<iCreateMessageForUserGroupCmd>(CmdTwo);
        }
        #endregion
    }
}
