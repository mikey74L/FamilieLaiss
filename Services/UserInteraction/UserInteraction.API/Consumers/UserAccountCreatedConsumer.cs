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
    /// MassTransit consumer for "UserAccountCreated"-Event
    /// </summary>
    public class UserAccountCreatedConsumer : IConsumer<iUserAccountCreatedEvent>
    {
        #region Private Members
        private readonly IMediator _Mediator;
        private readonly ILogger<UserAccountCreatedConsumer> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="mediator">The mediator. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public UserAccountCreatedConsumer(IMediator mediator, ILogger<UserAccountCreatedConsumer> logger)
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
        public async Task Consume(ConsumeContext<iUserAccountCreatedEvent> context)
        {
            //Ausgeben von Logging
            _Logger.LogInformation("Consumer for user account created was called with following parameters:");
            _Logger.LogDebug($"ID       : {context.Message.ID}");
            _Logger.LogDebug($"User-Name: {context.Message.UserName.Substring(1, 3)}");

            //Aufrufen des Mediator-Commands
            _Logger.LogDebug("Sending command for make user account with mediator");
            var Message = new MtrMakeUserAccountCmd(context.Message.ID, context.Message.UserName);
            await _Mediator.Send(Message);
        }
        #endregion
    }
}
