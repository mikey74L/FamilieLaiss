using DomainHelper.Interfaces;
using MediatR;
using Upload.API.GraphQL.Mutations.UploadPicture;

namespace Upload.API.Mediator.Commands.UploadPicture;

/// <summary>
/// Mediatr Command for delete all upload picture
/// </summary>
public class MtrDeleteAllUploadPicturesCmd : IRequest<IEnumerable<Domain.Entities.UploadPicture>>
{
    public required DeleteAllUploadPictureInput InputData { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for delete all upload picture
/// </summary>
/// <remarks>
/// Primary constructor 
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrDeleteAllUploadPictureCmdHandler(
    iUnitOfWork unitOfWork,
    ILogger<MtrDeleteAllUploadPictureCmdHandler> logger)
    : IRequestHandler<MtrDeleteAllUploadPicturesCmd, IEnumerable<Domain.Entities.UploadPicture>>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task<IEnumerable<Domain.Entities.UploadPicture>> Handle(MtrDeleteAllUploadPicturesCmd request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for delete all upload picture command was called for {@InputData}",
            request.InputData);

        logger.LogDebug("Get repository for upload picture");
        var repository = unitOfWork.GetRepository<Domain.Entities.UploadPicture>();

        logger.LogDebug("Get models to delete in store");
        var modelsToDelete = await repository.GetAll(x => request.InputData.UploadPictureIds.Contains(x.Id));

        logger.LogDebug("Delete upload picture domain models from store");
        foreach (var modelToDelete in modelsToDelete)
        {
            repository.Delete(modelToDelete);
        }

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();

        return modelsToDelete;
    }
}