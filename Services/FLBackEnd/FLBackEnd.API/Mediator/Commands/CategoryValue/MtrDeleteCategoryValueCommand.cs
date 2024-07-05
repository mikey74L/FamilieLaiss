using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using FLBackEnd.API.GraphQL.Mutations.CategoryValue;
using MediatR;

namespace FLBackEnd.API.Mediator.Commands.CategoryValue;

/// <summary>
/// Mediatr Command for delete category value
/// </summary>
public class MtrDeleteCategoryValueCommand : IRequest<Domain.Entities.CategoryValue>
{
    /// <summary>
    /// Input data from GraphQL
    /// </summary>
    public required DeleteCategoryValueInput InputData { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for delete category value
/// </summary>
/// <remarks>
/// Primary constructor
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrDeleteCategoryValueCommandHandler(
    iUnitOfWork unitOfWork,
    ILogger<MtrDeleteCategoryValueCommandHandler> logger)
    : IRequestHandler<MtrDeleteCategoryValueCommand, Domain.Entities.CategoryValue>
{
    #region Mediatr-Handler

    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task<Domain.Entities.CategoryValue> Handle(MtrDeleteCategoryValueCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for delete category value command was called for {@Input}",
            request.InputData);

        logger.LogDebug("Get repository for category - values");
        var repository = unitOfWork.GetRepository<Domain.Entities.CategoryValue>();

        logger.LogDebug("Find category value model in store");
        var modelToDelete = await repository.GetOneAsync(request.InputData.Id);
        if (modelToDelete == null)
        {
            logger.LogError("Could not find category value with id {ID}", request.InputData.Id);
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Could not find category value with id = {request.InputData.Id}");
        }

        logger.LogDebug("Get repository for category");
        var repositoryCategory = unitOfWork.GetRepository<Domain.Entities.Category>();

        logger.LogDebug("Find category model in store");
        var modelCategory = await repositoryCategory.GetOneAsync(modelToDelete.CategoryId);
        if (modelCategory == null)
        {
            logger.LogError("Could not find category with id {ID}", modelToDelete.CategoryId);
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Could not find category with id = {modelToDelete.CategoryId}");
        }

        logger.LogDebug("Delete category value from category");
        await modelCategory.RemoveCategoryValue(request.InputData.Id);

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();

        return modelToDelete;
    }

    #endregion
}