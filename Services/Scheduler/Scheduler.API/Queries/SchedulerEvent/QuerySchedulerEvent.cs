using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using FamilieLaissSharedObjects.Classes;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Scheduler.DTO.SchedulerEvent;
using Scheduler.DTO.SchedulerResource;
using ServiceLayerHelper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler.API.Queries.SchedulerEvent
{
    /// <summary>
    /// Query for get scheduler event
    /// </summary>
    public class QuerySchedulerEvent : IRequest<IEnumerable<SchedulerEventDTOModel>>
    {
        #region Properties
        /// <summary>
        /// Identifier for scheduler event (used when single entry is requested)
        /// </summary>
        public long? ID { get; private set; }

        /// <summary>
        /// Query Parameter
        /// </summary>
        public QueryParams Params { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="queryParams">Query Parameter</param>
        /// <param name="id">Identifier for scheduler event (used when single entry is requested)</param>
        public QuerySchedulerEvent(QueryParams queryParams, long? id = null)
        {
            Params = queryParams;
            ID = id;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr-Query-Handler for get scheduler event
    /// </summary>
    public class QueryHandlerSchedulerEvent : IRequestHandler<QuerySchedulerEvent, IEnumerable<SchedulerEventDTOModel>>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly ILogger<QueryHandlerSchedulerEvent> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public QueryHandlerSchedulerEvent(iUnitOfWork unitOfWork, ILogger<QueryHandlerSchedulerEvent> logger)
        {
            //Übernehmen der injected Objects
            _UnitOfWork = unitOfWork;
            _Logger = logger;
        }
        #endregion

        #region Query-Handler
        /// <summary>
        /// Will be called by Mediatr
        /// </summary>
        /// <param name="request">The request data</param>
        /// <param name="cancellationToken">The cancelation token</param>
        /// <returns>Task</returns>
        public async Task<IEnumerable<SchedulerEventDTOModel>> Handle(QuerySchedulerEvent request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Query-Handler for scheduler event was called with following parameters:");
            _Logger.LogDebug($"ID                   : {request.ID}");
            _Logger.LogDebug($"Where-Clause         : {request.Params.WhereClause}");
            _Logger.LogDebug($"Navigation-Properties: {request.Params.IncludeNav}");
            _Logger.LogDebug($"Order-By             : {request.Params.OrderBy}");
            _Logger.LogDebug($"Skip                 : {request.Params.Skip}");
            _Logger.LogDebug($"Take                 : {request.Params.Take}");

            //Repository ermitteln
            _Logger.LogDebug("Create repository");
            var Repository = _UnitOfWork.GetReadOnlyRepository<Scheduler.Domain.Aggregates.SchedulerEvent>();

            if (!request.ID.HasValue) //Wenn liste angefordert ist
            {
                //Ermitteln der Daten
                _Logger.LogDebug("Get data from repository");
                List<Scheduler.Domain.Aggregates.SchedulerEvent> Entities = await Repository.GetAll(null, request.Params.WhereClause, request.Params.IncludeNav, request.Params.OrderBy, request.Params.Take, request.Params.Skip);

                //Mit Automapper in DTOs umwandeln
                _Logger.LogDebug("Create DTO objects with automapper");
                var MappedEntities = Entities.Adapt<IEnumerable<SchedulerEventDTOModel>>();

                //Result zurückliefern
                _Logger.LogDebug("Result");
                return MappedEntities;
            }
            else //Wenn Single Entity angefordert ist
            {
                //Ermitteln der Entity aus dem Store
                _Logger.LogDebug("Get scheduler event from repository");
                var Entity = await Repository.GetOneAsync(request.ID.Value, request.Params.IncludeNav);
                if (Entity == null)
                {
                    throw new NoDataFoundException();
                }

                //Umwandeln des Items mit Automapper
                _Logger.LogDebug("Create DTO with automapper");
                var MappedEntity = Entity.Adapt<SchedulerEventDTOModel>();

                //Ergebnis zurückliefern
                _Logger.LogDebug("Result");
                return new List<SchedulerEventDTOModel>() { MappedEntity };
            }
        }
        #endregion
    }
}
