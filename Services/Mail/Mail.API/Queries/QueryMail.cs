using DomainHelper.Interfaces;
using FamilieLaissSharedObjects.Classes;
using Mail.API.Models.DTO;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceLayerHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mail.API.Queries
{
    /// <summary>
    /// Query for get Mail
    /// </summary>
    public class QueryMail : IRequest<IEnumerable<MailDTOModel>>
    {
        #region Properties
        /// <summary>
        /// Query Parameter
        /// </summary>
        public QueryParams Params { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="queryParams">Query-Parameter</param>
        public QueryMail(QueryParams queryParams)
        {
            Params = queryParams;
        }
        #endregion
    }
    /// <summary>
    /// Mediatr-Query-Handler for get Mail 
    /// </summary>
    public class QueryHandlerMail : IRequestHandler<QueryMail, IEnumerable<MailDTOModel>>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly ILogger<QueryHandlerMail> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public QueryHandlerMail(iUnitOfWork unitOfWork, ILogger<QueryHandlerMail> logger)
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
        public async Task<IEnumerable<MailDTOModel>> Handle(QueryMail request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Query-Handler for convert status was called with following parameters:");
            _Logger.LogDebug($"Where-Clause         : {request.Params.WhereClause}");
            _Logger.LogDebug($"Navigation-Properties: {request.Params.IncludeNav}");
            _Logger.LogDebug($"Order-By             : {request.Params.OrderBy}");
            _Logger.LogDebug($"Skip                 : {request.Params.Skip}");
            _Logger.LogDebug($"Take                 : {request.Params.Take}");

            //Repository ermitteln
            _Logger.LogDebug("Create repository");
            var Repository = _UnitOfWork.GetReadOnlyRepository<Mail.Domain.Entities.Mail>();

            //Ermitteln der Daten
            _Logger.LogDebug("Get data from repository");
            IEnumerable<Mail.Domain.Entities.Mail> Entities = await Repository.GetAll(null, request.Params.WhereClause, request.Params.IncludeNav, request.Params.OrderBy, request.Params.Take, request.Params.Skip);

            //Mit Automapper in DTOs umwandeln
            _Logger.LogDebug("Create DTOs with automapper");
            var MappedEntities = Entities.Adapt<IEnumerable<MailDTOModel>>();

            //Result zurückliefern
            _Logger.LogDebug("Return result");
            return MappedEntities;
        }
        #endregion
    }
}
