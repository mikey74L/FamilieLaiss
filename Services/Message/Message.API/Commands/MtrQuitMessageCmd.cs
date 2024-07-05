using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using MediatR;

namespace Message.API.Commands
{
    /// <summary>
    /// Mediatr Command for Quit Message
    /// </summary>
    public class MtrQuitMessageCmd : IRequest
    {
        #region Properties
        /// <summary>
        /// Identifier for Message
        /// </summary>
        public long ID { get; private set; }

        /// <summary>
        /// Username for current user
        /// </summary>
        public string UserName { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">Identifier for Message</param>
        /// <param name="userName">Username for current user</param>
        public MtrQuitMessageCmd(long id, string userName)
        {
            ID = id;
            UserName = userName;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for Quit Message command
    /// </summary>
    public class MtrQuitMessageCmdHandler : IRequestHandler<MtrQuitMessageCmd>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly ILogger<MtrQuitMessageCmdHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">Unit of work. Will be injected by DI-Container</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrQuitMessageCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrQuitMessageCmdHandler> logger)
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
        public async Task Handle(MtrQuitMessageCmd request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for quit message command was called with following parameters:");
            _Logger.LogDebug($"User-Name: {request.UserName.Substring(1, 3)}");
            _Logger.LogDebug($"ID       : {request.ID}");

            //Ermitteln des Repository
            _Logger.LogDebug("Get repository for message");
            var Repository = _UnitOfWork.GetRepository<Message.Domain.Aggregates.Message>();

            //Ermitteln der Nachricht
            _Logger.LogDebug("Find message");
            var Message = await Repository.GetOneAsync(request.ID);
            if (Message == null)
            {
                throw new NoDataFoundException($"Could not find message with id = {request.ID}");
            }

            //Quittieren der Nachricht
            _Logger.LogDebug("Quit message for user");
            await Message.SetMessageReaded(request.UserName);

            //Speichern der Änderungen
            _Logger.LogDebug("Save changes");
            await _UnitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}