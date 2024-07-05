using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using Mapster;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserInteraction.Domain.Aggregates;
using UserInteraction.DTO;

namespace UserInteraction.API.Commands
{
    /// <summary>
    /// Mediatr Command for create favorite
    /// </summary>
    public class MtrCreateFavoriteCmd : IRequest<FavoriteDTOModel>
    {
        #region Public Properties
        /// <summary>
        /// Identifier for user interaction info this favorite belongs to
        /// </summary>
        public long UserInteractionInfoID { get; private set; }

        /// <summary>
        /// Username of user that ths favorite belongs to
        /// </summary>
        public string UserName { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="userInteractionInfoID">Identifier for user interaction info</param>
        /// <param name="userName">The user name that this rating belongs to</param>
        public MtrCreateFavoriteCmd(long userInteractionInfoID, string userName)
        {
            UserInteractionInfoID = userInteractionInfoID;
            UserName = userName;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for create favorite command
    /// </summary>
    public class MtrCreateFavoriteCmdHandler : IRequestHandler<MtrCreateFavoriteCmd, FavoriteDTOModel>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly IBus _MassTransit;
        private readonly ILogger<MtrCreateFavoriteCmdHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="massTransit">The service bus. Will be injected by DI.</param>
        /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrCreateFavoriteCmdHandler(IBus massTransit, iUnitOfWork unitOfWork, ILogger<MtrCreateFavoriteCmdHandler> logger)
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
        public async Task<FavoriteDTOModel> Handle(MtrCreateFavoriteCmd request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for create favorite command was called with folowing parameters:");
            _Logger.LogDebug($"UserInteractionInfoID: {request.UserInteractionInfoID}");
            _Logger.LogDebug($"Username             : {request.UserName.Substring(1, 3)}");

            //Repository ermitteln
            _Logger.LogDebug("Get repository for UserInterActionInfo");
            var RepositoryInfo = _UnitOfWork.GetRepository<UserInteractionInfo>();

            //Repository ermitteln
            _Logger.LogDebug("Get repository for UserAccount");
            var RepositoryAccount = _UnitOfWork.GetRepository<UserAccount>();

            //Ermitteln der Entity
            _Logger.LogDebug("Get changing entity for user interaction info from repository");
            var Entity = await RepositoryInfo.GetOneAsync(request.UserInteractionInfoID);
            if (Entity == null)
            {
                throw new NoDataFoundException();
            }

            //Ermitteln der Entity
            _Logger.LogDebug("Get account entity from repository");
            var Accounts = await RepositoryAccount.GetAll(x => x.UserName == request.UserName);
            if (Accounts.Count == 0)
            {
                throw new NoDataFoundException();
            }

            //Adding comment to entity
            _Logger.LogDebug("Adding favorite to entity");
            var NewRating = Entity.AddFavorite(Accounts.First().Id);

            //Speichern der Daten
            _Logger.LogDebug("Saving changes to data store");
            await _UnitOfWork.SaveChangesAsync();

            //Umwandeln des Result-Wertes mit Automapper
            _Logger.LogDebug("Create result data with automapper");
            var Result = NewRating.Adapt<FavoriteDTOModel>();

            //Funktionsergebnis
            return Result;
        }
        #endregion
    }
}
