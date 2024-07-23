using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadPicture;
using MediatR;

namespace Catalog.API.Mediator.Commands.UploadPicture;

/// <summary>
/// Mediatr Command for delete upload picture
/// </summary>
public class MtrDeleteUploadPictureCmd : IRequest
{
    /// <summary>
    /// The message data from mass transit
    /// </summary>
    public required IMassUploadPictureDeletedEvent Message { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for delete upload picture command
/// </summary>
/// <remarks>
/// Primary constructor
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrDeleteUploadPictureCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrDeleteUploadPictureCmdHandler> logger)
    : IRequestHandler<MtrDeleteUploadPictureCmd>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task Handle(MtrDeleteUploadPictureCmd request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for delete upload picture command was called for {@Message}",
            request.Message);

        logger.LogDebug("Get repository for upload picture");
        var repository = unitOfWork.GetRepository<Domain.Entities.UploadPicture>();

        logger.LogDebug("Get model to delete in store");
        var modelToDelete = await repository.GetOneAsync(request.Message.Id);
        if (modelToDelete == null)
        {
            logger.LogError("Could not find upload picture with id {ID}", request.Message.Id);
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Could not find upload picture with id = {request.Message.Id}");
        }

        logger.LogDebug("Delete upload picture domain model from store");
        repository.Delete(modelToDelete);

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();
    }
}