using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using MediatR;

namespace Scheduler.API.Commands.SchedulerResource
{
    /// <summary>
    /// Mediatr Command for update scheduler resource
    /// </summary>
    public class MtrUpdateSchedulerResourceCmd : IRequest
    {
        #region Properties
        /// <summary>
        /// Identifier for scheduler resource
        /// </summary>
        public long ID { get; private set; }

        /// <summary>
        /// Name for this scheduler resource
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Display color for this scheduler resource
        /// </summary>
        public string Color { get; private set; }

        /// <summary>
        /// Starting hour for working day 
        /// </summary>
        public string StartingTime { get; private set; }

        /// <summary>
        /// Ending hour for working day
        /// </summary>
        public string EndingTime { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">Identifier for scheduler resource</param>
        /// <param name="name">Name for this scheduler resource</param>
        /// <param name="color">Display color for this scheduler resource</param>
        /// <param name="startingTime">Starting hour for working day </param>
        /// <param name="endingTime">Ending hour for working day</param>
        public MtrUpdateSchedulerResourceCmd(long id, string name, string color, string startingTime, string endingTime)
        {
            ID = id;
            Name = name;
            Color = color;
            StartingTime = startingTime;
            EndingTime = endingTime;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for update scheduler resource
    /// </summary>
    public class MtrUpdateSchedulerResourceCmdHandler : IRequestHandler<MtrUpdateSchedulerResourceCmd>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly ILogger<MtrUpdateSchedulerResourceCmdHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">Unit of work. Will be injected by DI-Container</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrUpdateSchedulerResourceCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrUpdateSchedulerResourceCmdHandler> logger)
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
        public async Task Handle(MtrUpdateSchedulerResourceCmd request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for update scheduler resource command was called with following parameters:");
            _Logger.LogDebug($"ID           : {request.ID}");
            _Logger.LogDebug($"Name         : {request.Name}");
            _Logger.LogDebug($"Color        : {request.Color}");
            _Logger.LogDebug($"Starting-Time: {request.StartingTime}");
            _Logger.LogDebug($"Ending-Time  : {request.EndingTime}");

            //Repository für BlogEntry ermitteln
            _Logger.LogDebug("Get repository for scheduler resource");
            var Repository = _UnitOfWork.GetRepository<Scheduler.Domain.Entities.SchedulerResource>();

            //Ermitteln des bisherigen Models
            _Logger.LogDebug("Find model to update in store");
            var ModelToUpdate = await Repository.GetOneAsync(request.ID);
            if (ModelToUpdate == null)
            {
                throw new NoDataFoundException($"Could not find scheduler resource with id = {request.ID}");
            }

            //Kategoriedaten aktualisieren
            _Logger.LogDebug("Update scheduler resource data");
            ModelToUpdate.Update(request.Name, request.Color, request.StartingTime, request.EndingTime);

            //Speichern der Daten
            _Logger.LogDebug("Saving changes to data store");
            await _UnitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}
