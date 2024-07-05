using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using MediatR;
using Upload.Domain.Entities;

namespace Upload.API.Mediator.Commands
{
    /// <summary>
    /// Mediatr Command for set status for picture to unassigned
    /// </summary>
    public class MtrSetPictureStateUnAssignedCmd : IRequest
    {
        #region Public Properties
        /// <summary>
        /// Identifier for upload picture
        /// </summary>
        public required long ID { get; init; }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for set status for picture to unassigned
    /// </summary>
    public class MtrSetPictureStateUnAssignedCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrSetPictureStateUnAssignedCmdHandler> logger) : IRequestHandler<MtrSetPictureStateUnAssignedCmd>
    {
        #region Mediatr-Handler
        /// <summary>
        /// Will be called by Mediatr
        /// </summary>
        /// <param name="request">The request data</param>
        /// <param name="cancellationToken">The cancelation token</param>
        /// <returns>Task</returns>
        public async Task Handle(MtrSetPictureStateUnAssignedCmd request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Mediatr-Handler for set picture state to unassigned command was called with ID = {request.ID}");

            //Repository ermitteln
            logger.LogDebug("Get repository for UploadPicture");
            var repository = unitOfWork.GetRepository<UploadPicture>();

            //Ermitteln des zu ändernden Items aus dem Store
            logger.LogDebug($"Get upload picture domain model with id = {request.ID} from data store");
            var itemToChange = await repository.GetOneAsync(request.ID);
            if (itemToChange == null)
            {
                throw new DomainException(DomainExceptionType.NoDataFound, $"Could not find upload picture with id = {request.ID}");
            }

            //Anpassen der Daten
            logger.LogDebug("Updating domain model");
            itemToChange.SetPictureStateToUnAssigned();

            //Speichern der Daten
            logger.LogDebug("Saving changes to data store");
            await unitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}