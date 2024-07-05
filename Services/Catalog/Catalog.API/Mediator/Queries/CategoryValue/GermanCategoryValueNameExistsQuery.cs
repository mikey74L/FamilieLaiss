using Catalog.DTO.CategoryValue;
using DomainHelper.Interfaces;
using MediatR;

namespace Catalog.API.Mediator.Queries.CategoryValue;

public class GermanCategoryValueNameExistsQuery : IRequest<bool>
{
    public required CheckCategoryValueNameExistsDTO InputData { get; init; }
}

public class GermanCategoryValueNameExistsQueryHandler(iUnitOfWork unitOfWork, ILogger<GermanCategoryValueNameExistsQueryHandler> logger) :
    IRequestHandler<GermanCategoryValueNameExistsQuery, bool>
{
    public async Task<bool> Handle(GermanCategoryValueNameExistsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for check german category value name already exists was called");

        logger.LogDebug("Get repository for category");
        var repository = unitOfWork.GetRepository<Domain.Aggregates.CategoryValue>();

        var count = await repository.GetCount(x => x.Id != request.InputData.Id && x.CategoryID == request.InputData.CategoryId && x.NameGerman == request.InputData.Name);

        return count > 0;
    }
}