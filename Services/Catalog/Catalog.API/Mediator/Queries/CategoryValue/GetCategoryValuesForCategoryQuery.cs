using Catalog.DTO.Category;
using Catalog.DTO.CategoryValue;
using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using Mapster;
using MediatR;

namespace Catalog.API.Mediator.Queries.CategoryValue;

public class GetCategoryValuesForCategoryQuery : IRequest<CategoryDTO>
{
    public required long Id { get; init; }
}

public class GetCategoryValuesForCategoryQueryHandler(iUnitOfWork unitOfWork, ILogger<GetCategoryValuesForCategoryQueryHandler> logger) :
    IRequestHandler<GetCategoryValuesForCategoryQuery, CategoryDTO>
{
    public async Task<CategoryDTO> Handle(GetCategoryValuesForCategoryQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for get category values for category query was called");

        logger.LogDebug("Get repository for category");
        var repositoryCategory = unitOfWork.GetRepository<Domain.Aggregates.Category>();

        logger.LogDebug("Get repository for category value");
        var repositoryCategoryValue = unitOfWork.GetRepository<Domain.Aggregates.CategoryValue>();

        logger.LogDebug("Get category from repository");
        var category = await repositoryCategory.GetOneAsync(request.Id);

        if (category is not null)
        {
            logger.LogDebug("Get category values from repository");
            var categoryValues = await repositoryCategoryValue.GetAll(x => x.CategoryID == request.Id);

            logger.LogDebug("Mapping with mapster");
            var categoryMapped = category.Adapt<CategoryDTO>();
            categoryMapped.CategoryValues = categoryValues.Adapt<IEnumerable<CategoryValueDTO>>();

            return categoryMapped;
        }
        else
        {
            logger.LogError("Could not find category with {ID}", request.Id);
            throw new DomainException(DomainExceptionType.NoDataFound, $"Could not find category with id = {request.Id}");
        }
    }
}