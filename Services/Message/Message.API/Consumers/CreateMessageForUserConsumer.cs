using DomainHelper.Interfaces;
using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using MassTransit;

namespace Message.API.Consumers
{
    /// <summary>
    /// MassTransit consumer for "CreateMessageForUser"-Command
    /// </summary>
    public class CreateMessageForUserConsumer : IConsumer<iCreateMessageForUserCmd>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly ILogger<CreateMessageForUserConsumer> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public CreateMessageForUserConsumer(iUnitOfWork unitOfWork, ILogger<CreateMessageForUserConsumer> logger)
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
        public async Task Consume(ConsumeContext<iCreateMessageForUserCmd> context)
        {
            //Ausgeben von Logging
            _Logger.LogInformation("Consumer for \"Create message for user\" command was called with following parameters:");
            _Logger.LogDebug($"User-Name      : {context.Message.UserName.Substring(1, 3)}");
            _Logger.LogDebug($"Message-Prio   : {context.Message.MessagePrio}");
            _Logger.LogDebug($"Text-German    : {context.Message.TextGerman}");
            _Logger.LogDebug($"Text-English   : {context.Message.TextEnglish}");
            _Logger.LogDebug($"Additional-Data: {context.Message.AdditionalData}");

            //Ermitteln des Repositories für die Nachricht
            _Logger.LogDebug("Get repository for message");
            var RepositoryMessage = _UnitOfWork.GetRepository<Message.Domain.Aggregates.Message>();

            //Erstellen der Nachricht
            _Logger.LogDebug("Add new message");
            var NewMessage = new Message.Domain.Aggregates.Message(context.Message.MessagePrio, context.Message.TextGerman,
                context.Message.TextEnglish, context.Message.AdditionalData);
            NewMessage.AddMessageUser(context.Message.UserName);
            await RepositoryMessage.AddAsync(NewMessage);

            //Speichern der Änderungen
            _Logger.LogDebug("Saving changes to store");
            await _UnitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}
