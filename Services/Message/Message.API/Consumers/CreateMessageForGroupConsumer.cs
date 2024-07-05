using DomainHelper.Interfaces;
using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using MassTransit;

namespace Message.API.Consumers
{
    /// <summary>
    /// MassTransit consumer for "CreateMessageForUserGroup"-Command
    /// </summary>
    public class CreateMessageForGroupConsumer : IConsumer<iCreateMessageForUserGroupCmd>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly ILogger<CreateMessageForGroupConsumer> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
        /// <param name="identityService">The HTTP-Client for identity microsservice. Will be injected by DI-Container</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public CreateMessageForGroupConsumer(iUnitOfWork unitOfWork, ILogger<CreateMessageForGroupConsumer> logger)
        {
            //Übernehmen der Injected Objects
            _UnitOfWork = unitOfWork;
            _Logger = logger;
        }
        #endregion

        #region Interface IConsumer
        /// <summary>
        /// Would be called from Masstransit
        /// </summary>
        /// <param name="context">The context for this event</param>
        /// <returns>Task</returns>
        public async Task Consume(ConsumeContext<iCreateMessageForUserGroupCmd> context)
        {
            //Ausgeben von Logging
            _Logger.LogInformation("Consumer for \"Create message for group\" command was called with following parameters:");
            _Logger.LogDebug($"Group-Name     : {context.Message.GroupName}");
            _Logger.LogDebug($"Message-Prio   : {context.Message.MessagePrio}");
            _Logger.LogDebug($"Text-German    : {context.Message.TextGerman}");
            _Logger.LogDebug($"Text-English   : {context.Message.TextEnglish}");
            _Logger.LogDebug($"Additional-Data: {context.Message.AdditionalData}");

            //Ermitteln der Benutzer für die Gruppe
            _Logger.LogDebug("Get users for group from identity microservice");
            //var Users = await _IdentityService.GetUsersForGroup(context.Message.GroupName);

            //Ermitteln des Repositories für die Nachricht
            _Logger.LogDebug("Get repository for message");
            var RepositoryMessage = _UnitOfWork.GetRepository<Message.Domain.Aggregates.Message>();

            //Erstellen der Nachricht
            _Logger.LogDebug("Add new message");
            var NewMessage = new Message.Domain.Aggregates.Message(context.Message.MessagePrio, context.Message.TextGerman,
                context.Message.TextEnglish, context.Message.AdditionalData);

            //Hinzufügen der Benutzer zur Message
            _Logger.LogDebug("Adding users to message");
            //foreach (var User in Users)
            //{
            //    NewMessage.AddMessageUser(User.UserName);
            //}

            //Hinzufügen der Nachricht zum Repository
            _Logger.LogDebug("Adding message to repository");
            await RepositoryMessage.AddAsync(NewMessage);

            //Speichern der Änderungen
            _Logger.LogDebug("Saving changes to store");
            await _UnitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}
