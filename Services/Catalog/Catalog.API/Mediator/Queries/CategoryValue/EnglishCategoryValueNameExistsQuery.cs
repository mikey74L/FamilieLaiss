using Catalog.DTO.CategoryValue;
using DomainHelper.Interfaces;
using MediatR;

namespace Catalog.API.Mediator.Queries.CategoryValue;

public class EnglishCategoryValueNameExistsQuery : IRequest<bool>
{
    public required CheckCategoryValueNameExistsDTO InputData { get; init; }
}

public class EnglishCategoryValueNameExistsQueryHandler(iUnitOfWork unitOfWork, ILogger<EnglishCategoryValueNameExistsQueryHandler> logger) :
    IRequestHandler<EnglishCategoryValueNameExistsQuery, bool>
{
    public async Task<bool> Handle(EnglishCategoryValueNameExistsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for check english category value name already exists was called");

        logger.LogDebug("Get repository for category");
        var repository = unitOfWork.GetRepository<Domain.Aggregates.CategoryValue>();

        var count = await repository.GetCount(x => x.Id != request.InputData.Id && x.CategoryID == request.InputData.CategoryId && x.NameEnglish == request.InputData.Name);

        return count > 0;
    }
}