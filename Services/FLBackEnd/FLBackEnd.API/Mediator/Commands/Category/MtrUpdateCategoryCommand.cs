using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using FLBackEnd.API.GraphQL.Mutations.Category;
using MediatR;

namespace FLBackEnd.API.Mediator.Commands.Category;

/// <summary>
/// Mediatr Command for update category 
/// </summary>
public class MtrUpdateCategoryCmd : IRequest<Domain.Entities.Category>
{
    /// <summary>
    /// GraphQL input data
    /// </summary>
    public required UpdateCategoryInput InputData { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for update category
/// </summary>
/// <remarks>
/// Primary constructor
/// </remarks>
/// <param name="unitOfWork">Unit of work. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrUpdateCategoryCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrUpdateCategoryCmdHandler> logger)
    : IRequestHandler<MtrUpdateCategoryCmd, Domain.Entities.Category>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task<Domain.Entities.Category> Handle(MtrUpdateCategoryCmd request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for update category command was called for {@InputData}",
            request.InputData);

        logger.LogDebug("Get repository for category");
        var repository = unitOfWork.GetRepository<Domain.Entities.Category>();

        logger.LogDebug("Get find model to update in store");
        var modelToUpdate = await repository.GetOneAsync(request.InputData.Id);
        if (modelToUpdate == null)
        {
            logger.LogError("Could not find category with {ID}", request.InputData.Id);
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Could not find category with id = {request.InputData.Id}");
        }

        logger.LogDebug("Update category data");
        modelToUpdate.Update(request.InputData.NameGerman, request.InputData.NameEnglish);
        repository.Update(modelToUpdate);

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();

        return modelToUpdate;
    }
}