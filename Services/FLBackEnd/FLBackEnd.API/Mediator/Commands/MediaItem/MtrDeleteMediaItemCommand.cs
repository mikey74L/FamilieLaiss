using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using FLBackEnd.API.GraphQL.Mutations.MediaItem;
using MediatR;

namespace FLBackEnd.API.Mediator.Commands.MediaItem;

/// <summary>
/// Mediatr Command for delete media item
/// </summary>
public class MtrDeleteMediaItemCommand : IRequest<Domain.Entities.MediaItem>
{
    #region Properties
    /// <summary>
    /// Input data from GraphQL
    /// </summary>
    public required DeleteMediaItemInput InputData { get; init; }
    #endregion
}

/// <summary>
/// Mediatr Command-Handler for delete media item
/// </summary>
public class MtrDeleteMediaItemCommandHandler(iUnitOfWork unitOfWork, ILogger<MtrDeleteMediaItemCommandHandler> logger) : IRequestHandler<MtrDeleteMediaItemCommand, Domain.Entities.MediaItem>
{
    #region Mediatr-Handler
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancelation token</param>
    /// <returns>Task</returns>
    public async Task<Domain.Entities.MediaItem> Handle(MtrDeleteMediaItemCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for delete media item command was called for {@Input}", request.InputData);

        logger.LogDebug("Set context parameter");
        unitOfWork.AddContextParameter("KeepUploadItem", request.InputData.KeepUploadItem);

        logger.LogDebug("Get repository for media group");
        var repository = unitOfWork.GetRepository<Domain.Entities.MediaGroup>();

        logger.LogDebug("Find model to delete in store");
        var mediaGroupModel = await repository.GetOneAsync(request.InputData.MediaGroupId);
        if (mediaGroupModel == null)
        {
            logger.LogError("Could not find media group with id {ID}", request.InputData.MediaGroupId);
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Could not find media group with id = {request.InputData.MediaGroupId}");
        }

        logger.LogDebug("Delete media group domain model from store");
        var modelToDelete = await mediaGroupModel.RemoveMediaItem(request.InputData.MediaItemId);

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();

        return modelToDelete;
    }
    #endregion
}
