using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using MediatR;

namespace Message.API.Commands
{
    /// <summary>
    /// Mediatr Command for quit message for all users
    /// </summary>
    public class MtrQuitMessageAllCmd : IRequest
    {
        #region Properties
        /// <summary>
        /// Identifier for Message
        /// </summary>
        public long ID { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">Identifier for Message</param>
        public MtrQuitMessageAllCmd(long id)
        {
            ID = id;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for quit message for all users
    /// </summary>
    public class MtrQuitMessageAllCmdHandler : IRequestHandler<MtrQuitMessageAllCmd>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly ILogger<MtrQuitMessageAllCmdHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">Unit of work. Will be injected by DI-Container</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrQuitMessageAllCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrQuitMessageAllCmdHandler> logger)
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
        public async Task Handle(MtrQuitMessageAllCmd request, CancellationToken cancellationToken)
        {
            _Logger.LogInformation("Mediatr-Handler for quit message for all users command was called with following parameters:");
            _Logger.LogDebug($"ID: {request.ID}");

            _Logger.LogDebug("Get repository for message");
            var Repository = _UnitOfWork.GetRepository<Message.Domain.Aggregates.Message>();

            _Logger.LogDebug("Find message");
            var Message = await Repository.GetOneAsync(request.ID);
            if (Message == null)
            {
                throw new NoDataFoundException($"Could not find message with id = {request.ID}");
            }

            _Logger.LogDebug("Quit message for all users");
            await Message.SetMessageReadedAll();

            _Logger.LogDebug("Save changes");
            await _UnitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}