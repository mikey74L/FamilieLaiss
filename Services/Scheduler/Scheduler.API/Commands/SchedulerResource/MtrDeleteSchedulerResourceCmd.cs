using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using MediatR;

namespace Scheduler.API.Commands.SchedulerResource
{
    /// <summary>
    /// Mediatr Command for delete scheduler resource
    /// </summary>
    public class MtrDeleteSchedulerResourceCmd : IRequest
    {
        #region Properties
        /// <summary>
        /// Identifier for scheduler resource
        /// </summary>
        public long ID { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">Identifier for scheduler resource</param>
        public MtrDeleteSchedulerResourceCmd(long id)
        {
            ID = id;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for delete scheduler resource
    /// </summary>
    public class MtrDeleteSchedulerResourceCmdHandler : IRequestHandler<MtrDeleteSchedulerResourceCmd>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly ILogger<MtrDeleteSchedulerResourceCmdHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">Unit of work. Will be injected by DI-Container</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrDeleteSchedulerResourceCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrDeleteSchedulerResourceCmdHandler> logger)
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
        public async Task Handle(MtrDeleteSchedulerResourceCmd request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for delete scheduler resource command was called with following parameters:");
            _Logger.LogDebug($"ID: {request.ID}");

            //Repository für Kategorie ermitteln
            _Logger.LogDebug("Get repository for scheduler resource");
            var Repository = _UnitOfWork.GetRepository<Scheduler.Domain.Entities.SchedulerResource>();

            //Ermitteln des bisherigen Models
            _Logger.LogDebug("Find model to delete in store");
            var ModelToDelete = await Repository.GetOneAsync(request.ID);
            if (ModelToDelete == null)
            {
                throw new NoDataFoundException($"Could not find scheduler resource with id = {request.ID}");
            }

            //Eine neue Kategorie erstellen
            _Logger.LogDebug("Delete scheduler resource domain model from store");
            Repository.Delete(ModelToDelete);

            //Speichern der Daten
            _Logger.LogDebug("Saving changes to data store");
            await _UnitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}
