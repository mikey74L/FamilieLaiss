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
    /// Mediatr Command for Make user interaction info
    /// </summary>
    public class MtrMakeUserInteractionInfoCmd : IRequest
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
        public MtrMakeUserInteractionInfoCmd(long id)
        {
            ID = id;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for Make user interaction info command
    /// </summary>
    public class MtrMakeUserInteractionInfoCmdHandler : IRequestHandler<MtrMakeUserInteractionInfoCmd>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly IBus _MassTransit;
        private readonly ILogger<MtrMakeUserInteractionInfoCmdHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="massTransit">The service bus. Will be injected by DI.</param>
        /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrMakeUserInteractionInfoCmdHandler(IBus massTransit, iUnitOfWork unitOfWork, ILogger<MtrMakeUserInteractionInfoCmdHandler> logger)
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
        public async Task Handle(MtrMakeUserInteractionInfoCmd request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for Make user interaction info command was called with following parameters:");
            _Logger.LogDebug($"ID: {request.ID}");

            //Repository ermitteln
            _Logger.LogDebug("Get repository for UserInteractionInfo");
            var Repository = _UnitOfWork.GetRepository<UserInteractionInfo>();

            //Ein neues Item erstellen
            _Logger.LogDebug("Adding new UserInteractionInfo domain model to repository");
            var Item = new UserInteractionInfo(request.ID);
            await Repository.AddAsync(Item);

            //Speichern der Daten
            _Logger.LogDebug("Saving changes to data store");
            await _UnitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}
