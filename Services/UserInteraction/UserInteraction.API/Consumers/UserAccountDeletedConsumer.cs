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
    /// MassTransit consumer for "UserAccountDeleted"-Event
    /// </summary>
    public class UserAccountDeletedConsumer : IConsumer<iUserAccountDeletedEvent>
    {
        #region Private Members
        private readonly IMediator _Mediator;
        private readonly ILogger<UserAccountDeletedConsumer> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="mediator">The mediator. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public UserAccountDeletedConsumer(IMediator mediator, ILogger<UserAccountDeletedConsumer> logger)
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
        public async Task Consume(ConsumeContext<iUserAccountDeletedEvent> context)
        {
            //Ausgeben von Logging
            _Logger.LogInformation("Consumer for user account deleted was called with following parameters:");
            _Logger.LogDebug($"ID: {context.Message.ID}");

            //Aufrufen des Mediator-Commands
            _Logger.LogDebug("Sending command for delete user account with mediator");
            var Message = new MtrDeleteUserAccountCmd(context.Message.ID);
            await _Mediator.Send(Message);
        }
        #endregion
    }
}
