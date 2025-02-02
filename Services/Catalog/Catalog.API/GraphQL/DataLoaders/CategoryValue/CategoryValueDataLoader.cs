using DomainHelper.Interfaces;

namespace Catalog.API.GraphQL.DataLoaders.CategoryValue;

public class CategoryValueDataLoader(
    iUnitOfWork unitOfWork,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null) : BatchDataLoader<long, Domain.Aggregates.CategoryValue>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<long, Domain.Aggregates.CategoryValue>> LoadBatchAsync(
        IReadOnlyList<long> keys, CancellationToken cancellationToken)
    {
        var readOnlyRepository = unitOfWork.GetReadOnlyRepository<Domain.Aggregates.CategoryValue>();

        var categoryValueItems = await readOnlyRepository.GetAll(x => keys.Contains(x.Id));

        return categoryValueItems.ToDictionary(x => x.Id);
    }
}