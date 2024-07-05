using DomainHelper.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UserInteraction.API.Queries
{
    /// <summary>
    /// Query for count comments
    /// </summary>
    public class QueryCountComment: IRequest<long>
    {
        #region Properties
        /// <summary>
        /// Username for user
        /// </summary>
        public string UserName { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="userName">Username for user</param>
        public QueryCountComment(string userName)
        {
            UserName = userName;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr-Query-Handler for count comments
    /// </summary>
    public class QueryHandlerCountComment : IRequestHandler<QueryCountComment, long>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly ILogger<QueryHandlerCountComment> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public QueryHandlerCountComment(iUnitOfWork unitOfWork, ILogger<QueryHandlerCountComment> logger)
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
        public async Task<long> Handle(QueryCountComment request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Query-Handler for count comments was called");

            //Repository ermitteln
            _Logger.LogDebug("Create repository");
            var Repository = _UnitOfWork.GetReadOnlyRepository<UserInteraction.Domain.Aggregates.Comment>();

            //Ermitteln der Anzahl
            _Logger.LogDebug("Get count of elements from store");
            var Result = await Repository.GetCount(x => x.UserAccount.UserName == request.UserName);

            //Ergebnis
            _Logger.LogDebug("Return result");
            return Result;
        }
        #endregion
    }
}
