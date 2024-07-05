using DomainHelper.Interfaces;
using FamilieLaissSharedObjects.Enums;
using MediatR;

namespace Message.API.Commands
{
    /// <summary>
    /// Mediatr Command for quit all messages
    /// </summary>
    public class MtrQuitAllMessagesCmd : IRequest
    {
        #region Properties
        /// <summary>
        /// Prio for Message
        /// </summary>
        public enMessagePrio Prio { get; private set; }

        /// <summary>
        /// Username for current user
        /// </summary>
        public string UserName { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="prio">Prio for Message</param>
        /// <param name="userName">Username for current user</param>
        public MtrQuitAllMessagesCmd(enMessagePrio prio, string userName)
        {
            Prio = prio;
            UserName = userName;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for quit message for all users
    /// </summary>
    public class MtrQuitAllMessagesCmdHandler : IRequestHandler<MtrQuitAllMessagesCmd>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly ILogger<MtrQuitAllMessagesCmdHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">Unit of work. Will be injected by DI-Container</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrQuitAllMessagesCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrQuitAllMessagesCmdHandler> logger)
        {
            _UnitOfWork = unitOfWork;
            _Logger = logger;
        }
        #endregion

        #region Mediatr-Handler
        /// <summary>
        /// Will be called by Mediatr
        /// </summary>
        /// <param name="request">The request data</param>
        /// <param name="cancellationToken">The cancelation token</param>
        /// <returns>Task</returns>
        public async Task Handle(MtrQuitAllMessagesCmd request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for quit all messages command was called with following parameters:");
            _Logger.LogDebug($"User-Name: {request.UserName.Substring(1, 3)}");
            _Logger.LogDebug($"Prio     : {request.Prio}");

            //Ermitteln des Repository
            _Logger.LogDebug("Get repository for message");
            var Repository = _UnitOfWork.GetRepository<Message.Domain.Aggregates.Message>();

            //Get all Messages
            _Logger.LogDebug("Find message");
            var Messages = await Repository.GetAll(x => x.Prio == request.Prio && x.MessageUsers.Any(y => y.UserName == request.UserName && !y.Readed));

            //Quittieren der Nachrichten
            _Logger.LogDebug("Quit message");
            foreach (var Item in Messages)
            {
                await Item.SetMessageReaded(request.UserName);
            }

            //Speichern der Änderungen
            _Logger.LogDebug("Save changes");
            await _UnitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}