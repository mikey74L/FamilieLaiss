using Catalog.DTO.CategoryValue;
using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using Mapster;
using MediatR;

namespace Catalog.API.Mediator.Queries.CategoryValue;

public class GetCategoryValueQuery : IRequest<CategoryValueDTO>
{
    public required long Id { get; init; }
}

public class GetCategoryValueQueryHandler(iUnitOfWork unitOfWork, ILogger<GetCategoryValueQueryHandler> logger) :
    IRequestHandler<GetCategoryValueQuery, CategoryValueDTO>
{
    public async Task<CategoryValueDTO> Handle(GetCategoryValueQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for get category value query was called");

        logger.LogDebug("Get repository for category value");
        var repository = unitOfWork.GetRepository<Domain.Aggregates.CategoryValue>();

        logger.LogDebug("Get category value from repository");
        var categoryValue = await repository.GetOneAsync(request.Id);

        if (categoryValue is not null)
        {
            logger.LogDebug("Mapping with mapster");
            var categoryValueMapped = categoryValue.Adapt<CategoryValueDTO>();

            return categoryValueMapped;
        }
        else
        {
            logger.LogError("Could not find category value with {ID}", request.Id);
            throw new DomainException(DomainExceptionType.NoDataFound, $"Could not find category value with id = {request.Id}");
        }
    }
}