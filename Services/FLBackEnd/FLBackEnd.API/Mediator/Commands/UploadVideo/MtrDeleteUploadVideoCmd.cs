using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using FLBackEnd.API.GraphQL.Mutations.UploadVideo;
using MediatR;

namespace FLBackEnd.API.Mediator.Commands.UploadVideo;

/// <summary>
/// Mediatr Command for delete upload video
/// </summary>
public class MtrDeleteUploadVideoCmd : IRequest<Domain.Entities.UploadVideo>
{
    /// <summary>
    /// InputData data from GraphQL
    /// </summary>
    public required DeleteUploadVideoInput InputData { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for delete upload video
/// </summary>
/// <remarks>
/// Primary constructor 
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrDeleteUploadVideoCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrDeleteUploadVideoCmdHandler> logger)
    : IRequestHandler<MtrDeleteUploadVideoCmd, Domain.Entities.UploadVideo>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task<Domain.Entities.UploadVideo> Handle(MtrDeleteUploadVideoCmd request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for delete upload video command was called for {@InputData}",
            request.InputData);

        logger.LogDebug("Get repository for upload video");
        var repository = unitOfWork.GetRepository<Domain.Entities.UploadVideo>();

        logger.LogDebug("Get model to delete in store");
        var modelToDelete = await repository.GetOneAsync(request.InputData.Id);
        if (modelToDelete == null)
        {
            logger.LogError("Could not find upload video with id {ID}", request.InputData.Id);
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Could not find upload video with id = {request.InputData.Id}");
        }

        logger.LogDebug("Delete upload video domain model from store");
        repository.Delete(modelToDelete);

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();

        return modelToDelete;
    }
}