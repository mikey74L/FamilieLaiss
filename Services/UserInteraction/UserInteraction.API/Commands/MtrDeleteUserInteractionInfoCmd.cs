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
    /// Mediatr Command for delete user interaction info
    /// </summary>
    public class MtrDeleteUserInteractionInfoCmd : IRequest
    {
        #region Public Properties
        /// <summary>
        /// The ID for the user interaction info
        /// </summary>
        public long ID { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">The ID for the user interaction info</param>
        public MtrDeleteUserInteractionInfoCmd(long id)
        {
            ID = id;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for delete user interaction info command
    /// </summary>
    public class MtrDeleteUserInteractionInfoCmdHandler : IRequestHandler<MtrDeleteUserInteractionInfoCmd>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly IBus _MassTransit;
        private readonly ILogger<MtrDeleteUserInteractionInfoCmdHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="massTransit">The service bus. Will be injected by DI.</param>
        /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrDeleteUserInteractionInfoCmdHandler(IBus massTransit, iUnitOfWork unitOfWork, ILogger<MtrDeleteUserInteractionInfoCmdHandler> logger)
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
        public async Task Handle(MtrDeleteUserInteractionInfoCmd request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for delete user interaction info command was called with following parameters:");
            _Logger.LogDebug($"ID: {request.ID}");

            //Repository ermitteln
            _Logger.LogDebug("Get repository for UserInteractionInfo");
            var Repository = _UnitOfWork.GetRepository<UserInteractionInfo>();

            //Ein neues Item erstellen
            _Logger.LogDebug("Get UserInteractionInfo domain model from repository");
            var Item = await Repository.GetOneAsync(request.ID);
            if (Item == null)
            {
                throw new NoDataFoundException($"UserInteractionInfo with id = {request.ID} could not be found");
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
