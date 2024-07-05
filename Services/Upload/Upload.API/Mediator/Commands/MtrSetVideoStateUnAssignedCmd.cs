using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using MediatR;
using Upload.Domain.Entities;

namespace Upload.API.Mediator.Commands
{
    /// <summary>
    /// Mediatr Command for set status for video to unassigned
    /// </summary>
    public class MtrSetVideoStateUnAssignedCmd : IRequest
    {
        #region Public Properties
        /// <summary>
        /// Identifier for upload video
        /// </summary>
        public required long ID { get; init; }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for set status for video to unassigned
    /// </summary>
    public class MtrSetVideoStateUnAssignedCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrSetVideoStateUnAssignedCmdHandler> logger) : IRequestHandler<MtrSetVideoStateUnAssignedCmd>
    {
        #region Mediatr-Handler
        /// <summary>
        /// Will be called by Mediatr
        /// </summary>
        /// <param name="request">The request data</param>
        /// <param name="cancellationToken">The cancelation token</param>
        /// <returns>Task</returns>
        public async Task Handle(MtrSetVideoStateUnAssignedCmd request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Mediatr-Handler for set video state to assigned command was called with ID = {request.ID}");

            //Repository ermitteln
            logger.LogDebug("Get repository for UploadVideo");
            var repository = unitOfWork.GetRepository<UploadVideo>();

            //Ermitteln des zu ändernden Items aus dem Store
            logger.LogDebug($"Get upload video domain model with id = {request.ID} from data store");
            var itemToChange = await repository.GetOneAsync(request.ID);
            if (itemToChange == null)
            {
                throw new DomainException(DomainExceptionType.NoDataFound, $"Could not find upload video with id = {request.ID}");
            }

            //Anpassen der Daten
            logger.LogDebug("Updating domain model");
            itemToChange.SetVideoStateToUnAssigned();

            //Speichern der Daten
            logger.LogDebug("Saving changes to data store");
            await unitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}