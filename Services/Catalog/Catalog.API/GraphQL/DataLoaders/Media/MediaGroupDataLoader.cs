using DomainHelper.Interfaces;

namespace Catalog.API.GraphQL.DataLoaders.Media;

public class MediaGroupDataLoader(
    iUnitOfWork unitOfWork,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null) : BatchDataLoader<long, Domain.Aggregates.MediaGroup>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<long, Domain.Aggregates.MediaGroup>> LoadBatchAsync(
        IReadOnlyList<long> keys, CancellationToken cancellationToken)
    {
        var readOnlyRepository = unitOfWork.GetReadOnlyRepository<Domain.Aggregates.MediaGroup>();

        var mediaGroupItems = await readOnlyRepository.GetAll(x => keys.Contains(x.Id));

        return mediaGroupItems.ToDictionary(x => x.Id);
    }
}