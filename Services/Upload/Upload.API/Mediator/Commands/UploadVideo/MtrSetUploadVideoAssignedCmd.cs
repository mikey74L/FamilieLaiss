using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using FamilieLaissMassTransitDefinitions.Contracts.Events.MediaItem;
using MediatR;

namespace Upload.API.Mediator.Commands.UploadVideo;

/// <summary>
/// Mediatr Command for set upload video to assigned
/// </summary>
public class MtrSetUploadVideoAssignedCmd : IRequest
{
    public required IMassMediaItemCreatedEvent InputData { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for set upload video to assigned
/// </summary>
/// <remarks>
/// Primary constructor 
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrSetUploadVideoAssignedCmdHandler(
    iUnitOfWork unitOfWork,
    ILogger<MtrSetUploadVideoAssignedCmdHandler> logger)
    : IRequestHandler<MtrSetUploadVideoAssignedCmd>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task Handle(MtrSetUploadVideoAssignedCmd request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Mediatr-Handler for set upload video to assigned command was called for {@InputData}",
            request.InputData);

        logger.LogDebug("Get repository for upload video");
        var repository = unitOfWork.GetRepository<Domain.Entities.UploadVideo>();

        logger.LogDebug("Get model to update in store");
        var modelToUpdate = await repository.GetOneAsync(request.InputData.UploadItemId);
        if (modelToUpdate == null)
        {
            logger.LogError("Could not find upload video with id {ID}", request.InputData.UploadItemId);
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Could not find upload video with id = {request.InputData.UploadItemId}");
        }

        logger.LogDebug("Set state to assigned");
        modelToUpdate.SetVideoStateToAssigned();

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();
    }
}