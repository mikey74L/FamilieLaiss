using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using FLBackEnd.API.GraphQL.Mutations.CategoryValue;
using MediatR;

namespace FLBackEnd.API.Mediator.Commands.CategoryValue;

/// <summary>
/// Mediatr Command for update category value
/// </summary>
public class MtrUpdateCategoryValueCommand : IRequest<Domain.Entities.CategoryValue>
{
    /// <summary>
    /// GraphQL input data
    /// </summary>
    public required UpdateCategoryValueInput InputData { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for update category value
/// </summary>
/// <remarks>
/// Primary constructor
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrUpdateCategoryValueCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrUpdateCategoryValueCmdHandler> logger)
    : IRequestHandler<MtrUpdateCategoryValueCommand, Domain.Entities.CategoryValue>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task<Domain.Entities.CategoryValue> Handle(MtrUpdateCategoryValueCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for update category value command was called for {@Input}",
            request.InputData);

        logger.LogDebug("Get repository for category value");
        var repository = unitOfWork.GetRepository<Domain.Entities.CategoryValue>();

        logger.LogDebug("Get find model to update in store");
        var modelToUpdate = await repository.GetOneAsync(request.InputData.Id);
        if (modelToUpdate == null)
        {
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Could not find category value with id = {request.InputData.Id}");
        }

        logger.LogDebug("Update category value data");
        modelToUpdate.Update(request.InputData.NameGerman, request.InputData.NameEnglish);
        repository.Update(modelToUpdate);

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();

        return modelToUpdate;
    }
}