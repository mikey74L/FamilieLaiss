using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using FamilieLaissMassTransitDefinitions.Contracts.Events;
using MediatR;

namespace Upload.API.Mediator.Commands.UploadPicture;

/// <summary>
/// Mediatr Command for set upload picture to unassigned
/// </summary>
public class MtrSetUploadPictureUnAssignedCmd : IRequest
{
    public required IMassMediaItemDeletedEvent InputData { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for set upload picture to unassigned
/// </summary>
/// <remarks>
/// Primary constructor 
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrSetUploadPictureUnAssignedCmdHandler(
    iUnitOfWork unitOfWork,
    ILogger<MtrSetUploadPictureUnAssignedCmdHandler> logger)
    : IRequestHandler<MtrSetUploadPictureUnAssignedCmd>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task Handle(MtrSetUploadPictureUnAssignedCmd request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Mediatr-Handler for set upload picture to unassigned command was called for {@InputData}",
            request.InputData);

        logger.LogDebug("Get repository for upload picture");
        var repository = unitOfWork.GetRepository<Domain.Entities.UploadPicture>();

        logger.LogDebug("Get model to update in store");
        var modelToUpdate = await repository.GetOneAsync(request.InputData.Id);
        if (modelToUpdate == null)
        {
            logger.LogError("Could not find upload picture with id {ID}", request.InputData.Id);
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Could not find upload picture with id = {request.InputData.Id}");
        }

        logger.LogDebug("Set state to unassigned");
        modelToUpdate.SetPictureStateToUnAssigned();

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();
    }
}