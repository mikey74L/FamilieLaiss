using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using FamilieLaissSharedObjects.Classes;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceLayerHelper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserInteraction.Domain.Aggregates;
using UserInteraction.DTO;

namespace UserInteraction.API.Queries
{
    /// <summary>
    /// Query for get favorites
    /// </summary>
    public class QueryFavorite : IRequest<IEnumerable<FavoriteDTOModel>>
    {
        #region Properties
        /// <summary>
        /// Identifier for favorite (used when single item is requested)
        /// </summary>
        public long? ID { get; private set; }

        /// <summary>
        /// Identifier for user interaction info
        /// </summary>
        public long? InteractionInfoID { get; private set; }

        /// <summary>
        /// Username for user
        /// </summary>
        public string UserName { get; private set; }

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
        /// <param name="id">Identifier for favorite (used when single item is requested)</param>
        /// <param name="interactionInfoID">Identifier for user interaction info</param>
        /// <param name="userName">Username for user</param>
        public QueryFavorite(QueryParams queryParams, long? id = null, long? interactionInfoID = null, string userName = null)
        {
            Params = queryParams;
            ID = id;
            InteractionInfoID = interactionInfoID;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr-Query-Handler for get favorites
    /// </summary>
    public class QueryHanderFavorite : IRequestHandler<QueryFavorite, IEnumerable<FavoriteDTOModel>>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly ILogger<QueryHanderFavorite> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public QueryHanderFavorite(iUnitOfWork unitOfWork, ILogger<QueryHanderFavorite> logger)
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
        public async Task<IEnumerable<FavoriteDTOModel>> Handle(QueryFavorite request, CancellationToken cancellationToken)
        {
            return null;
            ////Logging ausgeben
            //_Logger.LogInformation($"Mediatr-Query-Handler for favorite with id = {request.ID} was called with following parameters:");
            //_Logger.LogDebug($"Where-Clause         : {request.Params.WhereClause}");
            //_Logger.LogDebug($"Navigation-Properties: {request.Params.IncludeNav}");
            //_Logger.LogDebug($"Order-By             : {request.Params.OrderBy}");
            //_Logger.LogDebug($"Skip                 : {request.Params.Skip}");
            //_Logger.LogDebug($"Take                 : {request.Params.Take}");

            ////Repository ermitteln
            //_Logger.LogDebug("Create repository");
            //var Repository = _UnitOfWork.GetReadOnlyRepository<Favorite>();

            //if (!request.ID.HasValue && !request.InteractionInfoID.HasValue && string.IsNullOrEmpty(request.UserName))   //Liste aller Comments für alle Vater-Objekte und alle User
            //{
            //    //Ermitteln der Daten
            //    _Logger.LogDebug("Get data from repository");
            //    List<Favorite> Entities = await Repository.GetAll(null, request.Params.WhereClause, "UserAccount", request.Params.OrderBy, request.Params.Take, request.Params.Skip);

            //    //Mit Automapper in DTOs umwandeln
            //    _Logger.LogDebug("Create DTO objects with automapper");
            //    var MappedEntities = _Mapper.Map<IEnumerable<Favorite>, IEnumerable<FavoriteDTOModel>>(Entities,
            //        opt => {
            //            opt.AfterMap((source, dest) =>
            //            {
            //                //Nach dem Mapping den Benutzernamen aus dem UserAccount holen und zum DTO hinzufügen
            //                foreach (var DestItem in dest)
            //                {
            //                    foreach (var SourceItem in source)
            //                    {
            //                        if (SourceItem.Id == DestItem.ID)
            //                        {
            //                            DestItem.UserName = SourceItem.UserAccount.UserName;
            //                            break;
            //                        }
            //                    }
            //                }
            //            });
            //        });


            //    //Result zurückliefern
            //    _Logger.LogDebug("Result");
            //    return MappedEntities;
            //}
            //else 
            //{
            //    if (request.ID.HasValue)  //Ein Comment mit dem Identifier spezifiziert
            //    {
            //        //Ermitteln der Entity aus dem Store
            //        _Logger.LogDebug("Get user interaction info from repository");
            //        var Entity = await Repository.GetOneAsync(request.ID.Value, "UserAccount");
            //        if (Entity == null)
            //        {
            //            throw new NoDataFoundException();
            //        }

            //        //Umwandeln des Items mit Automapper
            //        _Logger.LogDebug("Create DTO with automapper");
            //        var MappedEntity = _Mapper.Map<Favorite, FavoriteDTOModel>(Entity,
            //            //Nach dem Mapping den Benutzernamen aus dem UserAccount holen und zum DTO hinzufügen
            //            opt =>
            //            {
            //                opt.AfterMap((source, dest) =>
            //                {
            //                    dest.UserName = source.UserAccount.UserName;
            //                });
            //            });


            //        //Ergebnis zurückliefern
            //        _Logger.LogDebug("Result");
            //        return new List<FavoriteDTOModel>() { MappedEntity };
            //    }
            //    else
            //    {
            //        //Ermitteln der Daten
            //        _Logger.LogDebug("Get data from repository");
            //        List<Favorite> Entities = await Repository.GetAll(x => x.UserAccount.UserName == request.UserName, "", "UserAccount", request.Params.OrderBy, request.Params.Take, request.Params.Skip);

            //        //Mit Automapper in DTOs umwandeln
            //        _Logger.LogDebug("Create DTO objects with automapper");
            //        var MappedEntities = _Mapper.Map<IEnumerable<Favorite>, IEnumerable<FavoriteDTOModel>>(Entities,
            //            opt =>
            //            {
            //                opt.AfterMap((source, dest) =>
            //                {
            //                        //Nach dem Mapping den Benutzernamen aus dem UserAccount holen und zum DTO hinzufügen
            //                        foreach (var DestItem in dest)
            //                    {
            //                        foreach (var SourceItem in source)
            //                        {
            //                            if (SourceItem.Id == DestItem.ID)
            //                            {
            //                                DestItem.UserName = SourceItem.UserAccount.UserName;
            //                                break;
            //                            }
            //                        }
            //                    }
            //                });
            //            });

            //        //Result zurückliefern
            //        _Logger.LogDebug("Result");
            //        return MappedEntities;
            //    }
            //}
        }
        #endregion
    }
}
