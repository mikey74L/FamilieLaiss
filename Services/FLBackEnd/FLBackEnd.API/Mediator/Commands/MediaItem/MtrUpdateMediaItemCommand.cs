using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using FLBackEnd.API.GraphQL.Mutations.MediaItem;
using MediatR;

namespace FLBackEnd.API.Mediator.Commands.MediaItem;

/// <summary>
/// Mediatr Command for update media item 
/// </summary>
public class MtrUpdateMediaItemCommand : IRequest<Domain.Entities.MediaItem>
{
    #region Properties
    /// <summary>
    /// GraphQL input data
    /// </summary>
    public required UpdateMediaItemInput InputData { get; init; }
    #endregion
}

/// <summary>
/// Mediatr Command-Handler for update media item
/// </summary>
public class MtrUpdateMediaItemCommandHandler(iUnitOfWork unitOfWork, ILogger<MtrUpdateMediaItemCommandHandler> logger) : IRequestHandler<MtrUpdateMediaItemCommand, Domain.Entities.MediaItem>
{
    #region Mediatr-Handler
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancelation token</param>
    /// <returns>Task</returns>
    public async Task<Domain.Entities.MediaItem> Handle(MtrUpdateMediaItemCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for update media item command was called for {@Input}", request.InputData);

        logger.LogDebug("Get repository for media item");
        var repository = unitOfWork.GetRepository<Domain.Entities.MediaItem>();

        logger.LogDebug("Find model to update in store");
        var modelToUpdate = await repository.GetOneAsync(request.InputData.Id);
        if (modelToUpdate == null)
        {
            logger.LogError("Could not find media item with {ID}", request.InputData.Id);
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Could not find media item with id = {request.InputData.Id}");
        }

        logger.LogDebug("Update media item data");
        modelToUpdate.Update(request.InputData.NameGerman, request.InputData.NameEnglish, request.InputData.DescriptionGerman,
            request.InputData.DescriptionEnglish, request.InputData.OnlyFamily);

        logger.LogDebug("Update assigned category values for media item");
        await modelToUpdate.UpdateCategoryValues(request.InputData.CategoryValueIds);

        logger.LogDebug("Saving changes to data store");
        repository.Update(modelToUpdate);
        await unitOfWork.SaveChangesAsync();

        return modelToUpdate;
    }
    #endregion
}
