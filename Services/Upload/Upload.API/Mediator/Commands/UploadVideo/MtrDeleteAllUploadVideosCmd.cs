using DomainHelper.Interfaces;
using MediatR;
using Upload.API.GraphQL.Mutations.UploadVideo;

namespace Upload.API.Mediator.Commands.UploadVideo;

/// <summary>
/// Mediatr Command for delete all upload video
/// </summary>
public class MtrDeleteAllUploadVideosCmd : IRequest<IEnumerable<Domain.Entities.UploadVideo>>
{
    public required DeleteAllUploadVideoInput InputData { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for delete all upload video
/// </summary>
/// <remarks>
/// Primary constructor 
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrDeleteAllUploadVideoCmdHandler(
    iUnitOfWork unitOfWork,
    ILogger<MtrDeleteAllUploadVideoCmdHandler> logger)
    : IRequestHandler<MtrDeleteAllUploadVideosCmd, IEnumerable<Domain.Entities.UploadVideo>>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task<IEnumerable<Domain.Entities.UploadVideo>> Handle(MtrDeleteAllUploadVideosCmd request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for delete all upload video command was called for {@InputData}",
            request.InputData);

        logger.LogDebug("Get repository for upload video");
        var repository = unitOfWork.GetRepository<Domain.Entities.UploadVideo>();

        logger.LogDebug("Get models to delete in store");
        var modelsToDelete = await repository.GetAll(x => request.InputData.UploadVideoIds.Contains(x.Id));

        logger.LogDebug("Delete upload video domain models from store");
        foreach (var modelToDelete in modelsToDelete)
        {
            repository.Delete(modelToDelete);
        }

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();

        return modelsToDelete;
    }
}