using Catalog.DTO.CategoryValue;
using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using Mapster;
using MediatR;

namespace Catalog.API.Mediator.Commands.CategoryValue;

/// <summary>
/// Mediatr Command for delete category value
/// </summary>
public class MtrDeleteCategoryValueCmd : IRequest<CategoryValueDTO>
{
    #region Properties
    public long Id { get; init; }
    #endregion
}

/// <summary>
/// Mediatr Command-Handler for delete category value
/// </summary>
public class MtrDeleteCategoryValueCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrDeleteCategoryValueCmdHandler> logger) : IRequestHandler<MtrDeleteCategoryValueCmd, CategoryValueDTO>
{
    #region Mediatr-Handler
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancelation token</param>
    /// <returns>Task</returns>
    public async Task<CategoryValueDTO> Handle(MtrDeleteCategoryValueCmd request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for delete category value command was called for {@Input}", request.Id);

        logger.LogDebug("Get repository for category - values");
        var repository = unitOfWork.GetRepository<Domain.Aggregates.CategoryValue>();

        logger.LogDebug("Find category value model in store");
        var modelToDelete = await repository.GetOneAsync(request.Id);
        if (modelToDelete == null)
        {
            logger.LogError("Could not find category value with id {ID}", request.Id);
            throw new DomainException(DomainExceptionType.NoDataFound, $"Could not find category value with id = {request.Id}");
        }

        logger.LogDebug("Get repository for category");
        var repositoryCategory = unitOfWork.GetRepository<Domain.Aggregates.Category>();

        logger.LogDebug("Find category model in store");
        var modelCategory = await repositoryCategory.GetOneAsync(modelToDelete.CategoryID);
        if (modelCategory == null)
        {
            logger.LogError("Could not find category with id {ID}", modelToDelete.CategoryID);
            throw new DomainException(DomainExceptionType.NoDataFound, $"Could not find category with id = {modelToDelete.CategoryID}");
        }

        logger.LogDebug("Delete category value from category");
        await modelCategory.RemoveCategoryValue(request.Id);

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();

        logger.LogDebug("Mapping with mapster");
        var modelToDeleteMapped = modelToDelete.Adapt<CategoryValueDTO>();

        return modelToDeleteMapped;
    }
    #endregion
}
