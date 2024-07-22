using Catalog.API.GraphQL.Mutations.CategoryValue;
using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using MediatR;

namespace Catalog.API.Mediator.Commands.CategoryValue;

/// <summary>
/// Mediatr Command for add category value entry
/// </summary>
public class MtrAddCategoryValueCommand : IRequest<Domain.Aggregates.CategoryValue>
{
    /// <summary>
    /// The input data from GraphQL Mutation
    /// </summary>
    public required AddCategoryValueInput InputData { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for Make new category value entry command
/// </summary>
/// <remarks>
/// Primary constructor
/// </remarks>
/// <param name="unitOfWork">Unit of work. Will be injected by DI</param>
/// <param name="logger">Logger. Will be injected by DI</param>
public class MtrAddCategoryValueCommandHandler(
    iUnitOfWork unitOfWork,
    ILogger<MtrAddCategoryValueCommandHandler> logger)
    : IRequestHandler<MtrAddCategoryValueCommand, Domain.Aggregates.CategoryValue>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task<Domain.Aggregates.CategoryValue> Handle(MtrAddCategoryValueCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for make new category value entry command was called for {@Input}",
            request.InputData);

        logger.LogDebug("Get repository for category");
        var repository = unitOfWork.GetRepository<Domain.Aggregates.Category>();

        logger.LogDebug("Get category from store");
        var categoryEntity = await repository.GetOneAsync(request.InputData.CategoryId);
        if (categoryEntity == null)
        {
            logger.LogError("Could not find category with {ID}", request.InputData.CategoryId);
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Could not find category with id = {request.InputData.CategoryId}");
        }

        logger.LogDebug("Adding category value to category");
        var addedEntity = categoryEntity.AddCategoryValue(request.InputData.NameGerman, request.InputData.NameEnglish);

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();

        return addedEntity;
    }
}