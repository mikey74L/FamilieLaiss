using Catalog.DTO.CategoryValue;
using DomainHelper.Interfaces;
using Mapster;
using MediatR;

namespace Catalog.API.Mediator.Queries.CategoryValue;

public class GetAllCategoryValuesQuery : IRequest<IEnumerable<CategoryValueDTO>>
{
}

public class GetAllCategoryValuesQueryHandler(iUnitOfWork unitOfWork, ILogger<GetAllCategoryValuesQuery> logger) :
    IRequestHandler<GetAllCategoryValuesQuery, IEnumerable<CategoryValueDTO>>
{
    public async Task<IEnumerable<CategoryValueDTO>> Handle(GetAllCategoryValuesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for get all category values query was called");

        logger.LogDebug("Get repository for category value");
        var repository = unitOfWork.GetRepository<Domain.Aggregates.CategoryValue>();

        logger.LogDebug("Get all category values from repository");
        var allCategoryValues = await repository.GetAll();

        logger.LogDebug("Mapping with mapster");
        var mappedCategoryValues = allCategoryValues.Adapt<IEnumerable<CategoryValueDTO>>();

        return mappedCategoryValues;
    }
}