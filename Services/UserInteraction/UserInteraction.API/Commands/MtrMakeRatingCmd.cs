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
    /// Mediatr Command for create rating
    /// </summary>
    public class MtrMakeNewRatingCmd : IRequest<RatingDTOModel>
    {
        #region Public Properties
        /// <summary>
        /// Identifier for user interaction info this rating belongs to
        /// </summary>
        public long UserInteractionInfoID { get; private set; }

        /// <summary>
        /// Username of user that ths rating belongs to
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// The rating value
        /// </summary>
        public int Value { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="userInteractionInfoID">Identifier for user interaction info</param>
        /// <param name="userName">The user name that this rating belongs to</param>
        /// <param name="value">The rating value</param>
        public MtrMakeNewRatingCmd(long userInteractionInfoID, string userName, int value)
        {
            UserInteractionInfoID = userInteractionInfoID;
            UserName = userName;
            Value = value;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for create rating command
    /// </summary>
    public class MtrMakeRatingCmdHandler : IRequestHandler<MtrMakeNewRatingCmd, RatingDTOModel>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly IBus _MassTransit;
        private readonly ILogger<MtrMakeRatingCmdHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="massTransit">The service bus. Will be injected by DI.</param>
        /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrMakeRatingCmdHandler(IBus massTransit, iUnitOfWork unitOfWork, ILogger<MtrMakeRatingCmdHandler> logger)
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
        public async Task<RatingDTOModel> Handle(MtrMakeNewRatingCmd request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for create rating command was called with following parameters:");
            _Logger.LogDebug($"UserInteractionInfoID: {request.UserInteractionInfoID}");
            _Logger.LogDebug($"User-Name            : {request.UserName.Substring(1, 3)}");
            _Logger.LogDebug($"Value                : {request.Value}");

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
            _Logger.LogDebug("Get account entity for from repository");
            var Accounts = await RepositoryAccount.GetAll(x => x.UserName == request.UserName);
            if (Accounts.Count == 0)
            {
                throw new NoDataFoundException();
            }

            //Adding rating to entity
            _Logger.LogDebug("Adding rating to entity");
            var NewRating = Entity.AddRating(Accounts.First().Id, request.Value);

            //Speichern der Daten
            _Logger.LogDebug("Saving changes to data store");
            await _UnitOfWork.SaveChangesAsync();

            //Umwandeln des Result-Wertes mit Automapper
            _Logger.LogDebug("Create result data with automapper");
            var Result = NewRating.Adapt<RatingDTOModel>();

            //Funktionsergebnis
            return Result;
        }
        #endregion
    }
}
