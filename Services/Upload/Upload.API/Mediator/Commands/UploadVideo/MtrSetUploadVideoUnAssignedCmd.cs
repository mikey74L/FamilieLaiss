using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using FamilieLaissMassTransitDefinitions.Contracts.Events.MediaItem;
using MediatR;

namespace Upload.API.Mediator.Commands.UploadVideo;

/// <summary>
/// Mediatr Command for set upload video to unassigned
/// </summary>
public class MtrSetUploadVideoUnAssignedCmd : IRequest
{
    public required IMassMediaItemDeletedEvent InputData { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for set upload video to unassigned
/// </summary>
/// <remarks>
/// Primary constructor 
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrSetUploadVideoUnAssignedCmdHandler(
    iUnitOfWork unitOfWork,
    ILogger<MtrSetUploadVideoUnAssignedCmdHandler> logger)
    : IRequestHandler<MtrSetUploadVideoUnAssignedCmd>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task Handle(MtrSetUploadVideoUnAssignedCmd request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Mediatr-Handler for set upload video to unassigned command was called for {@InputData}",
            request.InputData);

        logger.LogDebug("Get repository for upload video");
        var repository = unitOfWork.GetRepository<Domain.Entities.UploadVideo>();

        logger.LogDebug("Get model to update in store");
        var modelToUpdate = await repository.GetOneAsync(request.InputData.Id);
        if (modelToUpdate == null)
        {
            logger.LogError("Could not find upload video with id {ID}", request.InputData.Id);
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Could not find upload video with id = {request.InputData.Id}");
        }

        logger.LogDebug("Set state to unassigned");
        modelToUpdate.SetVideoStateToUnAssigned();

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();
    }
}