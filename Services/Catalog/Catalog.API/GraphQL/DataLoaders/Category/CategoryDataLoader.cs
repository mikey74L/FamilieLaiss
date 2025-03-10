using DomainHelper.Interfaces;

namespace Catalog.API.GraphQL.DataLoaders.Category;

public class CategoryDataLoader(
    iUnitOfWork unitOfWork,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null) : BatchDataLoader<long, Domain.Aggregates.Category>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<long, Domain.Aggregates.Category>> LoadBatchAsync(
        IReadOnlyList<long> keys, CancellationToken cancellationToken)
    {
        var readOnlyRepository = unitOfWork.GetReadOnlyRepository<Domain.Aggregates.Category>();

        var categoryItems = await readOnlyRepository.GetAll(x => keys.Contains(x.Id));

        return categoryItems.ToDictionary(x => x.Id);
    }
}