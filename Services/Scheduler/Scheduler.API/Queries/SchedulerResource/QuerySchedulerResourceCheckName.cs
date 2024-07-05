using Scheduler.Domain.Entities;
using DomainHelper.Interfaces;
using FamilieLaissCoreHelpers.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Scheduler.API.Queries.SchedulerResource
{
    /// <summary>
    /// Query for check scheduler resource name
    /// </summary>
    public class QuerySchedulerResourceCheckName : IRequest<bool>
    {
        #region Properties
        /// <summary>
        /// The ID for a item to be checked in an update scenario
        /// </summary>
        public long ID { get; private set; }

        /// <summary>
        /// Additional information
        /// </summary>
        public long AdditionalType { get; private set; }

        /// <summary>
        /// The value to be checked
        /// </summary>
        public string Value { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">The ID for a item to be checked in an update scenario</param>
        /// <param name="additionalType">Additional information</param>
        /// <param name="value">The value to be checked</param>
        public QuerySchedulerResourceCheckName(long id, long additionalType, string value)
        {
            ID = id;
            AdditionalType = additionalType;
            Value = value;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr-Query-Handler for check scheduler resource name
    /// </summary>
    public class QueryHandlerSchedulerResourceCheckName : IRequestHandler<QuerySchedulerResourceCheckName, bool>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly ILogger<QueryHandlerSchedulerResourceCheckName> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public QueryHandlerSchedulerResourceCheckName(iUnitOfWork unitOfWork, ILogger<QueryHandlerSchedulerResourceCheckName> logger)
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
        public async Task<bool> Handle(QuerySchedulerResourceCheckName request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Query-Handler for check scheduler resource name was called with following parameters:");
            _Logger.LogDebug($"ID             : {request.ID}");
            _Logger.LogDebug($"Value          : {request.Value}");
            _Logger.LogDebug($"Additional-Type: {request.AdditionalType}");

            //Repository ermitteln
            _Logger.LogDebug("Create repository");
            var Repository = _UnitOfWork.GetReadOnlyRepository<Scheduler.Domain.Entities.SchedulerResource>();

            //Deklaration
            _Logger.LogDebug("Check if name already exists");
            int Anzahl = 0;

            if (request.ID == -1)
            {
                Anzahl = (await Repository.GetAll(x => x.Name == request.Value)).Count;
            }
            else
            {
                Anzahl = (await Repository.GetAll(x => x.Name == request.Value && x.Id != request.ID)).Count;
            }

            //Result zurückliefern
            if (Anzahl == 0)
            {
                _Logger.LogDebug("Name does not exist");
                return true;
            }
            else
            {
                _Logger.LogDebug("Name does already exist");
                return false;
            }
        }
        #endregion
    }
}
