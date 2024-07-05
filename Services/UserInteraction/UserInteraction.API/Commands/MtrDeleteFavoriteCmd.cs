using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using UserInteraction.Domain.Aggregates;

namespace UserInteraction.API.Commands
{
    /// <summary>
    /// Mediatr Command for delete favorite
    /// </summary>
    public class MtrDeleteFavoriteCmd : IRequest
    {
        #region Public Properties
        /// <summary>
        /// Identifier for favorite
        /// </summary>
        public long ID { get; private set; }

        /// <summary>
        /// The Identifier for user interaction info
        /// </summary>
        public long UserInteractionInfoID { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">Identifier for favorite</param>
        /// <param name="userInteractionInfoID">Identifier for user interaction info</param>
        public MtrDeleteFavoriteCmd(long id, long userInteractionInfoID)
        {
            ID = id;
            UserInteractionInfoID = userInteractionInfoID;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for delete user account command
    /// </summary>
    public class MtrDeleteFavoriteCmdHandler : IRequestHandler<MtrDeleteFavoriteCmd>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly IBus _MassTransit;
        private readonly ILogger<MtrDeleteFavoriteCmdHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="massTransit">The service bus. Will be injected by DI.</param>
        /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrDeleteFavoriteCmdHandler(IBus massTransit, iUnitOfWork unitOfWork, ILogger<MtrDeleteFavoriteCmdHandler> logger)
        {
            //Übernehmen der injected Classes
            _UnitOfWork = unitOfWork;
            _MassTransit = massTransit;
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
        public async Task Handle(MtrDeleteFavoriteCmd request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler für delete favorite command was called with following parameters:");
            _Logger.LogDebug($"ID                   : {request.ID}");
            _Logger.LogDebug($"UserInteractionInfoID: {request.UserInteractionInfoID}");

            //Repository ermitteln
            _Logger.LogDebug("Get repository for UserInteractionInfo");
            var Repository = _UnitOfWork.GetRepository<UserInteractionInfo>();

            //Item zum Löschen ermitteln
            _Logger.LogDebug("Get UserInteractionInfo domain model from repository");
            var Item = await Repository.GetOneAsync(request.UserInteractionInfoID);
            if (Item == null)
            {
                throw new NoDataFoundException($"UserInteractionInfo with id = {request.UserInteractionInfoID} could not be found");
            }

            //Entfernen des Items
            _Logger.LogDebug("Delete Item from repository");
            await Item.RemoveFavorite(request.ID);

            //Speichern der Daten
            _Logger.LogDebug("Saving changes to data store");
            await _UnitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}
