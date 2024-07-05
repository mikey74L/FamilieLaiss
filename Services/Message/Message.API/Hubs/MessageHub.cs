using FamilieLaissSharedObjects.Enums;
using MediatR;
using Message.API.Commands;
using Message.API.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Message.API.Hubs
{
    /// <summary>
    /// SignalR-Hub for Messages
    /// </summary>
    [Authorize("MessagePolicy")]
    public class MessageHub: Hub
    {
        #region Private Members
        private readonly IMediator _Mediator;
        #endregion

        #region C'tor
        public MessageHub(IMediator mediator)
        {
            _Mediator = mediator;
        }
        #endregion

        #region Hub-Methods
        /// <summary>
        /// Get the messages for prio = 1 (Error-Messages)
        /// </summary>
        /// <returns>The list of not already readed error messages for the user</returns>
        public async Task<IEnumerable<MessageDTOModel>> GetMessagesError()
        {
            //Zusammenstellen der Query
            var Query = new QueryMessage(enMessagePrio.Error, Context.UserIdentifier);

            //Ausführen der Query
            var Result = await _Mediator.Send<IEnumerable<MessageDTOModel>>(Query);

            //Automapping
            return Result;
        }

        /// <summary>
        /// Get the messages for prio = 2 (Warning-Messages)
        /// </summary>
        /// <returns>The list of not already readed warning messages for the user</returns>
        public async Task<IEnumerable<MessageDTOModel>> GetMessagesWarning()
        {
            //Zusammenstellen der Query
            var Query = new QueryMessage(enMessagePrio.Warning, Context.UserIdentifier);

            //Ausführen der Query
            var Result = await _Mediator.Send<IEnumerable<MessageDTOModel>>(Query);

            //Automapping
            return Result;
        }

        /// <summary>
        /// Get the messages for prio = 3 (Info-Messages)
        /// </summary>
        /// <returns>The list of not already readed info messages for the user</returns>
        public async Task<IEnumerable<MessageDTOModel>> GetMessagesInfo()
        {
            //Zusammenstellen der Query
            var Query = new QueryMessage(enMessagePrio.Info, Context.UserIdentifier);

            //Ausführen der Query
            var Result = await _Mediator.Send<IEnumerable<MessageDTOModel>>(Query);

            //Automapping
            return Result;
        }

        /// <summary>
        /// Quit a message for the current user
        /// </summary>
        /// <param name="id">The identifier for the message</param>
        public async Task QuitMessage(long id)
        {
            //Zusammenstellen des Commands
            var Command = new MtrQuitMessageCmd(id, Context.UserIdentifier);

            //Aufrufen des Commands
            await _Mediator.Send(Command);
        }

        /// <summary>
        /// Quit a message for all users
        /// </summary>
        /// <param name="id">The identifier for the message</param>
        public async Task QuitMessageAllUsers(long id)
        {
            //Zusammenstellen des Commands
            var Command = new MtrQuitMessageAllCmd(id);

            //Aufrufen des Commands
            await _Mediator.Send(Command);
        }

        /// <summary>
        /// Quit all messages for the current user
        /// </summary>
        /// <param name="prio">The message priority for which the messages should be quitted</param>
        public async Task QuitAllMessages(enMessagePrio prio)
        {
            //Zusammenstellen des Commands
            var Command = new MtrQuitAllMessagesCmd(prio, Context.UserIdentifier);

            //Aufrufen des Commands
            await _Mediator.Send(Command);
        }
        #endregion
    }
}
