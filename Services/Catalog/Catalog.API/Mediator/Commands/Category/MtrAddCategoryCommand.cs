using Catalog.API.GraphQL.Mutations.Category;
using DomainHelper.Interfaces;
using MediatR;

namespace Catalog.API.Mediator.Commands.Category;

/// <summary>
/// Mediatr Command for add category entry
/// </summary>
public class MtrAddCategoryCommand : IRequest<Domain.Aggregates.Category>
{
    /// <summary>
    /// The input data from GraphQL Mutation
    /// </summary>
    public required AddCategoryInput InputData { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for add category entry command
/// </summary>
/// <remarks>
/// Primary constructor
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrAddCategoryCommandHandler(iUnitOfWork unitOfWork, ILogger<MtrAddCategoryCommandHandler> logger)
    : IRequestHandler<MtrAddCategoryCommand, Domain.Aggregates.Category>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task<Domain.Aggregates.Category> Handle(MtrAddCategoryCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for make new category entry command was called for {@InputData}",
            request.InputData);

        logger.LogDebug("Get repository for category");
        var repository = unitOfWork.GetRepository<Domain.Aggregates.Category>();

        logger.LogDebug("Adding new category domain model to repository");
        var newCategory = new Domain.Aggregates.Category(request.InputData.CategoryType, request.InputData.NameGerman,
            request.InputData.NameEnglish);
        await repository.AddAsync(newCategory);

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();

        return newCategory;
    }
}