using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using FLBackEnd.API.GraphQL.Mutations.Category;
using MediatR;

namespace FLBackEnd.API.Mediator.Commands.Category;

/// <summary>
/// Mediatr Command for delete category
/// </summary>
public class MtrDeleteCategoryCmd : IRequest<Domain.Entities.Category>
{
    /// <summary>
    /// InputData data from GraphQL
    /// </summary>
    public required DeleteCategoryInput InputData { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for delete category
/// </summary>
/// <remarks>
/// Primary constructor 
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrDeleteCategoryCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrDeleteCategoryCmdHandler> logger)
    : IRequestHandler<MtrDeleteCategoryCmd, Domain.Entities.Category>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task<Domain.Entities.Category> Handle(MtrDeleteCategoryCmd request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for delete category command was called for {@InputData}",
            request.InputData);

        logger.LogDebug("Get repository for category");
        var repository = unitOfWork.GetRepository<Domain.Entities.Category>();

        logger.LogDebug("Get model to delete in store");
        var modelToDelete = await repository.GetOneAsync(request.InputData.Id);
        if (modelToDelete == null)
        {
            logger.LogError("Could not find category with id {ID}", request.InputData.Id);
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Could not find category with id = {request.InputData.Id}");
        }

        logger.LogDebug("Delete category domain model from store");
        repository.Delete(modelToDelete);

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();

        return modelToDelete;
    }
}