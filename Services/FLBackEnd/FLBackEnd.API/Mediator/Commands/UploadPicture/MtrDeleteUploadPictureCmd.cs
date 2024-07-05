using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using FLBackEnd.API.GraphQL.Mutations.UploadPicture;
using MediatR;

namespace FLBackEnd.API.Mediator.Commands.UploadPicture;

/// <summary>
/// Mediatr Command for delete upload picture
/// </summary>
public class MtrDeleteUploadPictureCmd : IRequest<Domain.Entities.UploadPicture>
{
    /// <summary>
    /// InputData data from GraphQL
    /// </summary>
    public required DeleteUploadPictureInput InputData { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for delete upload picture
/// </summary>
/// <remarks>
/// Primary constructor 
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrDeleteUploadPictureCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrDeleteUploadPictureCmdHandler> logger)
    : IRequestHandler<MtrDeleteUploadPictureCmd, Domain.Entities.UploadPicture>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task<Domain.Entities.UploadPicture> Handle(MtrDeleteUploadPictureCmd request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for delete upload picture command was called for {@InputData}",
            request.InputData);

        logger.LogDebug("Get repository for upload picture");
        var repository = unitOfWork.GetRepository<Domain.Entities.UploadPicture>();

        logger.LogDebug("Get model to delete in store");
        var modelToDelete = await repository.GetOneAsync(request.InputData.Id);
        if (modelToDelete == null)
        {
            logger.LogError("Could not find upload picture with id {ID}", request.InputData.Id);
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Could not find upload picture with id = {request.InputData.Id}");
        }

        logger.LogDebug("Delete upload picture domain model from store");
        repository.Delete(modelToDelete);

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();

        return modelToDelete;
    }
}