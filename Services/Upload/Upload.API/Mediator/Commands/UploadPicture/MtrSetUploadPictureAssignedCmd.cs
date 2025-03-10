using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using FamilieLaissMassTransitDefinitions.Contracts.Events.MediaItem;
using MediatR;

namespace Upload.API.Mediator.Commands.UploadPicture;

/// <summary>
/// Mediatr Command for set upload picture to assigned
/// </summary>
public class MtrSetUploadPictureAssignedCmd : IRequest
{
    public required IMassMediaItemCreatedEvent InputData { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for set upload picture to assigned
/// </summary>
/// <remarks>
/// Primary constructor 
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrSetUploadPictureAssignedCmdHandler(
    iUnitOfWork unitOfWork,
    ILogger<MtrSetUploadPictureAssignedCmdHandler> logger)
    : IRequestHandler<MtrSetUploadPictureAssignedCmd>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task Handle(MtrSetUploadPictureAssignedCmd request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Mediatr-Handler for set upload picture to assigned command was called for {@InputData}",
            request.InputData);

        logger.LogDebug("Get repository for upload picture");
        var repository = unitOfWork.GetRepository<Domain.Entities.UploadPicture>();

        logger.LogDebug("Get model to update in store");
        var modelToUpdate = await repository.GetOneAsync(request.InputData.UploadItemId);
        if (modelToUpdate == null)
        {
            logger.LogError("Could not find upload picture with id {ID}", request.InputData.UploadItemId);
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Could not find upload picture with id = {request.InputData.UploadItemId}");
        }

        logger.LogDebug("Set state to assigned");
        modelToUpdate.SetPictureStateToAssigned();

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();
    }
}