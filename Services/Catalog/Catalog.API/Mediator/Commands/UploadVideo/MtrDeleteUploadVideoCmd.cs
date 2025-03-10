using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadVideo;
using MediatR;

namespace Catalog.API.Mediator.Commands.UploadVideo;

/// <summary>
/// Mediatr Command for delete upload video
/// </summary>
public class MtrDeleteUploadVideoCmd : IRequest
{
    /// <summary>
    /// The message data from mass transit
    /// </summary>
    public required IMassUploadVideoDeletedEvent Message { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for delete upload video command
/// </summary>
/// <remarks>
/// Primary constructor
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrDeleteUploadVideoCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrDeleteUploadVideoCmdHandler> logger)
    : IRequestHandler<MtrDeleteUploadVideoCmd>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task Handle(MtrDeleteUploadVideoCmd request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for delete upload video command was called for {@Message}",
            request.Message);

        logger.LogDebug("Get repository for upload video");
        var repository = unitOfWork.GetRepository<Domain.Entities.UploadVideo>();

        logger.LogDebug("Get model to delete in store");
        var modelToDelete = await repository.GetOneAsync(request.Message.Id);
        if (modelToDelete == null)
        {
            logger.LogError("Could not find upload video with id {ID}", request.Message.Id);
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Could not find upload video with id = {request.Message.Id}");
        }

        logger.LogDebug("Delete upload video domain model from store");
        repository.Delete(modelToDelete);

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();
    }
}
