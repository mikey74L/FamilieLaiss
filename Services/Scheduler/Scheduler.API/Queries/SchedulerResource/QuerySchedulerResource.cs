using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using FamilieLaissSharedObjects.Classes;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Scheduler.Domain.Entities;
using Scheduler.DTO.SchedulerResource;
using ServiceLayerHelper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler.API.Queries.SchedulerResource
{
    /// <summary>
    /// Query for get scheduler resource
    /// </summary>
    public class QuerySchedulerResource : IRequest<IEnumerable<SchedulerResourceDTOModel>>
    {
        #region Properties
        /// <summary>
        /// Identifier for scheduler resource (used when single blog entry is requested)
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
        /// <param name="id">Identifier for scheduler resource (used when single blog entry is requested)</param>
        public QuerySchedulerResource(QueryParams queryParams, long? id = null)
        {
            Params = queryParams;
            ID = id;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr-Query-Handler for get scheduler resource
    /// </summary>
    public class QueryHandlerSchedulerResource : IRequestHandler<QuerySchedulerResource, IEnumerable<SchedulerResourceDTOModel>>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly ILogger<QueryHandlerSchedulerResource> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public QueryHandlerSchedulerResource(iUnitOfWork unitOfWork, ILogger<QueryHandlerSchedulerResource> logger)
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
        public async Task<IEnumerable<SchedulerResourceDTOModel>> Handle(QuerySchedulerResource request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Query-Handler for scheduler resource was called with following parameters:");
            _Logger.LogDebug($"ID                   : {request.ID}");
            _Logger.LogDebug($"Where-Clause         : {request.Params.WhereClause}");
            _Logger.LogDebug($"Navigation-Properties: {request.Params.IncludeNav}");
            _Logger.LogDebug($"Order-By             : {request.Params.OrderBy}");
            _Logger.LogDebug($"Skip                 : {request.Params.Skip}");
            _Logger.LogDebug($"Take                 : {request.Params.Take}");

            //Repository ermitteln
            _Logger.LogDebug("Create repository");
            var Repository = _UnitOfWork.GetReadOnlyRepository<Scheduler.Domain.Entities.SchedulerResource>();

            if (!request.ID.HasValue) //Wenn liste angefordert ist
            {
                //Ermitteln der Daten
                _Logger.LogDebug("Get data from repository");
                List<Scheduler.Domain.Entities.SchedulerResource> Entities = await Repository.GetAll(null, request.Params.WhereClause, request.Params.IncludeNav, request.Params.OrderBy, request.Params.Take, request.Params.Skip);

                //Mit Automapper in DTOs umwandeln
                _Logger.LogDebug("Create DTO objects with automapper");
                var MappedEntities = Entities.Adapt<IEnumerable<SchedulerResourceDTOModel>>();

                //Result zurückliefern
                _Logger.LogDebug("Result");
                return MappedEntities;
            }
            else //Wenn Single Entity angefordert ist
            {
                //Ermitteln der Entity aus dem Store
                _Logger.LogDebug("Get blog entry from repository");
                var Entity = await Repository.GetOneAsync(request.ID.Value, request.Params.IncludeNav);
                if (Entity == null)
                {
                    throw new NoDataFoundException();
                }

                //Umwandeln des Items mit Automapper
                _Logger.LogDebug("Create DTO with automapper");
                var MappedEntity = Entity.Adapt<SchedulerResourceDTOModel>();

                //Ergebnis zurückliefern
                _Logger.LogDebug("Result");
                return new List<SchedulerResourceDTOModel>() { MappedEntity };
            }
        }
        #endregion
    }
}
