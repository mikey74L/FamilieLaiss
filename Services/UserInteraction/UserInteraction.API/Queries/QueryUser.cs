using DomainHelper.Interfaces;
using InfrastructureHelper.Exceptions;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserInteraction.Domain.Aggregates;
using UserInteraction.DTO;

namespace UserInteraction.API.Queries
{
    /// <summary>
    /// Query for get user 
    /// </summary>
    public class QueryUser : IRequest<IEnumerable<FamilieLaissUserDTOModel>>
    {
        #region Properties
        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="userName">User name</param>
        public QueryUser(string userName)
        {
            UserName = userName;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr-Query-Handler for get user 
    /// </summary>
    public class QueryHandlerUser : IRequestHandler<QueryUser, IEnumerable<FamilieLaissUserDTOModel>>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly ILogger<QueryHandlerUser> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">The Unit of work. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public QueryHandlerUser(iUnitOfWork unitOfWork, ILogger<QueryHandlerUser> logger)
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
        public async Task<IEnumerable<FamilieLaissUserDTOModel>> Handle(QueryUser request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Query-Handler for get user has been called with following parameters:");
            _Logger.LogDebug($"User-Name: {(string.IsNullOrEmpty(request.UserName) ? "Not set" : request.UserName.Substring(1, 3))}");

            //Repository ermitteln
            _Logger.LogDebug("Create repository");
            var Repository = _UnitOfWork.GetReadOnlyRepository<UserAccount>();

            //Ermitteln der Daten
            _Logger.LogDebug("Get data from repository");
            List<UserAccount> Entities = null;
            if (!string.IsNullOrEmpty(request.UserName))
            {
                Entities = await Repository.GetAll(x => x.UserName == request.UserName);
                if (Entities.Count == 0)
                {
                    throw new DomainNotFoundException();
                }
            }
            else
            {
                Entities = await Repository.GetAll();
            }

            //Mit Automapper in DTOs umwandeln
            _Logger.LogDebug("Create DTO with automapper");
            var MappedEntities = Entities.Adapt<IEnumerable<FamilieLaissUserDTOModel>>();

            //Result zurückliefern
            _Logger.LogDebug("Return result");
            return MappedEntities;
        }
        #endregion
    }
}
