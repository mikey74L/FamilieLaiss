using DomainHelper.Exceptions;
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
    /// Mediatr Command for delete user account
    /// </summary>
    public class MtrDeleteUserAccountCmd : IRequest
    {
        #region Public Properties
        /// <summary>
        /// The ID for the user account
        /// </summary>
        public string ID { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">The ID for the user account</param>
        public MtrDeleteUserAccountCmd(string id)
        {
            ID = id;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for delete user account command
    /// </summary>
    public class MtrDeleteUserAccountCmdHandler : IRequestHandler<MtrDeleteUserAccountCmd>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly IBus _MassTransit;
        private readonly ILogger<MtrDeleteUserAccountCmdHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="massTransit">The service bus. Will be injected by DI.</param>
        /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrDeleteUserAccountCmdHandler(IBus massTransit, iUnitOfWork unitOfWork, ILogger<MtrDeleteUserAccountCmdHandler> logger)
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
        public async Task Handle(MtrDeleteUserAccountCmd request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for delete user account command was called with following parameters:");
            _Logger.LogDebug($"ID: {request.ID}");

            //Repository ermitteln
            _Logger.LogDebug("Get repository for UserAccount");
            var Repository = _UnitOfWork.GetRepository<UserAccount>();

            //Item zum Löschen ermitteln
            _Logger.LogDebug("Get UserAccount domain model from repository");
            var Item = await Repository.GetOneAsync(request.ID);
            if (Item == null)
            {
                throw new NoDataFoundException($"UserAccount with id = {request.ID} could not be found");
            }

            //Entfernen des Items
            _Logger.LogDebug("Delete Item from repository");
            Repository.Delete(Item);

            //Speichern der Daten
            _Logger.LogDebug("Saving changes to data store");
            await _UnitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}
