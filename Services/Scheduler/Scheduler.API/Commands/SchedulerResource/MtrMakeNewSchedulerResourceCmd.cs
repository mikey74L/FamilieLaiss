using DomainHelper.Interfaces;
using Mapster;
using MediatR;
using Scheduler.DTO.SchedulerResource;

namespace Scheduler.API.Commands.SchedulerResource
{
    /// <summary>
    /// Mediatr Command for make new scheduler resource
    /// </summary>
    public class MtrMakeNewSchedulerResourceCmd : IRequest<SchedulerResourceDTOModel>
    {
        #region Properties
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
        /// <param name="name">Name for this scheduler resource</param>
        /// <param name="color">Display color for this scheduler resource</param>
        /// <param name="startingTime">Starting hour for working day </param>
        /// <param name="endingTime">Ending hour for working day</param>
        public MtrMakeNewSchedulerResourceCmd(string name, string color, string startingTime, string endingTime)
        {
            Name = name;
            Color = color;
            StartingTime = startingTime;
            EndingTime = endingTime;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for Make new scheduler resource command
    /// </summary>
    public class MtrMakeNewSchedulerResourceCmdHandler : IRequestHandler<MtrMakeNewSchedulerResourceCmd, SchedulerResourceDTOModel>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly ILogger<MtrMakeNewSchedulerResourceCmdHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">Unit of Work. Will be injected by DI-Container</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrMakeNewSchedulerResourceCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrMakeNewSchedulerResourceCmdHandler> logger)
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
        public async Task<SchedulerResourceDTOModel> Handle(MtrMakeNewSchedulerResourceCmd request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for make new scheduler resource command was called with following parameters:");
            _Logger.LogDebug($"Name         : {request.Name}");
            _Logger.LogDebug($"Color        : {request.Color}");
            _Logger.LogDebug($"Starting-Time: {request.StartingTime}");
            _Logger.LogDebug($"Ending-Time  : {request.EndingTime}");

            //Repository für Kategorie ermitteln
            _Logger.LogDebug("Get repository for scheduler resources");
            var Repository = _UnitOfWork.GetRepository<Scheduler.Domain.Entities.SchedulerResource>();

            //Eine neue Kategorie erstellen
            _Logger.LogDebug("Adding new scheduler resource domain model to repository");
            var NewSchedulerResource = new Scheduler.Domain.Entities.SchedulerResource(request.Name, request.Color, request.StartingTime, request.EndingTime);
            await Repository.AddAsync(NewSchedulerResource);

            //Speichern der Daten
            _Logger.LogDebug("Saving changes to data store");
            await _UnitOfWork.SaveChangesAsync();

            //Umwandeln des Result-Wertes mit Automapper
            _Logger.LogDebug("Create result data with automapper");
            var Result = NewSchedulerResource.Adapt<SchedulerResourceDTOModel>();

            //Funktionsergebnis
            return Result;
        }
        #endregion
    }
}
