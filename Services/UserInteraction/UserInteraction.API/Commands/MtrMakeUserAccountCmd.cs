using DomainHelper.Interfaces;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using UserInteraction.Domain.Aggregates;

namespace UserInteraction.API.Commands
{
    /// <summary>
    /// Mediatr Command for Make user account
    /// </summary>
    public class MtrMakeUserAccountCmd : IRequest
    {
        #region Public Properties
        /// <summary>
        /// The ID for the user account
        /// </summary>
        public string ID { get; init; }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; init; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">The ID for the user account</param>
        /// <param name="userName">The user name for the account</param>
        public MtrMakeUserAccountCmd(string id, string userName)
        {
            ID = id;
            UserName = userName;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for Make user account command
    /// </summary>
    public class MtrMakeUserAccountCmdHandler : IRequestHandler<MtrMakeUserAccountCmd>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly IBus _MassTransit;
        private readonly ILogger<MtrMakeUserAccountCmdHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="massTransit">The service bus. Will be injected by DI.</param>
        /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrMakeUserAccountCmdHandler(IBus massTransit, iUnitOfWork unitOfWork, ILogger<MtrMakeUserAccountCmdHandler> logger)
        {
            //Übernehmen der injected Classes
            _UnitOfWork = unitOfWork;
            _MassTransit = massTransit;
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
        public async Task Handle(MtrMakeUserAccountCmd request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for Make user account command was called with following parameters:");
            _Logger.LogDebug($"ID       : {request.ID}");
            _Logger.LogDebug($"User-Name: {request.UserName.Substring(1, 3)}");

            //Repository ermitteln
            _Logger.LogDebug("Get repository for UserAccount");
            var Repository = _UnitOfWork.GetRepository<UserAccount>();

            //Ein neues Item erstellen
            _Logger.LogDebug("Adding new UserAccount domain model to repository");
            var Item = new UserAccount(request.ID, request.UserName);
            _ = await Repository.AddAsync(Item);

            //Speichern der Daten
            _Logger.LogDebug("Saving changes to data store");
            _ = await _UnitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}
