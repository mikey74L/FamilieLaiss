using DomainHelper.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using UserInteraction.Domain.Aggregates;
using UserInteraction.Domain.DomainEvents;

namespace UserInteraction.API.Events.MediaItem
{
    /// <summary>
    /// Event handler for favorite deleted
    /// </summary>
    public class MtrEventHandlerFavoriteDeleted : INotificationHandler<MtrEventFavoriteDeleted>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly ILogger<MtrEventHandlerFavoriteDeleted> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">Unit of work. Will be injected by DI-Container</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrEventHandlerFavoriteDeleted(iUnitOfWork unitOfWork, ILogger<MtrEventHandlerFavoriteDeleted> logger)
        {
            _UnitOfWork = unitOfWork;
            _Logger = logger;
        }
        #endregion

        #region Mediatr-Handler
        /// <summary>
        /// Will be called by Mediatr
        /// </summary>
        /// <param name="notification">The notification data</param>
        /// <param name="cancellationToken">The cancelation token</param>
        /// <returns>Task</returns>
        public async Task Handle(MtrEventFavoriteDeleted notification, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for favorite deleted was called with following parameters:");
            _Logger.LogDebug($"ID                   : {notification.ID}");
            _Logger.LogDebug($"UserAccountID        : {notification.UserAccountID}");
            _Logger.LogDebug($"UserInteractionInfoID: {notification.UserInteractionInfoID}");

            //Ermitteln des Repository für die UserInteractionInfo
            _Logger.LogDebug("Get repository for user interaction info");
            var RepositoryInfo = _UnitOfWork.GetRepository<UserInteractionInfo>();

            //Ermitteln des Repository für den UserAccount
            _Logger.LogDebug("Get repository for user account");
            var RepositoryAccount = _UnitOfWork.GetRepository<UserAccount>();

            //Ermitteln der Entity
            _Logger.LogDebug("Get entity from info repository");
            var InfoEntity = await RepositoryInfo.GetOneAsync(notification.UserInteractionInfoID);

            //Ermitteln der Entity
            _Logger.LogDebug("Get entity from account repository");
            var AccountEntity = await RepositoryAccount.GetOneAsync(notification.UserAccountID);

            //Update Info-Entity
            if (InfoEntity != null)
            {
                _Logger.LogDebug("Update info entity");
                await InfoEntity.UpdateFavoriteInfo();
            }

            //Update Account-Entity
            if (AccountEntity != null)
            {
                _Logger.LogDebug("Update account entity");
                await AccountEntity.UpdateFavoriteInfo();
            }

            //Saving Changes
            _Logger.LogDebug("Saving changes");
            await _UnitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}
