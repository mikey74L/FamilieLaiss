using DomainHelper.Interfaces;

namespace Catalog.API.GraphQL.DataLoaders.Media;

public class MediaItemDataLoader(
    iUnitOfWork unitOfWork,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null) : BatchDataLoader<long, Domain.Aggregates.MediaItem>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<long, Domain.Aggregates.MediaItem>> LoadBatchAsync(
        IReadOnlyList<long> keys, CancellationToken cancellationToken)
    {
        var readOnlyRepository = unitOfWork.GetReadOnlyRepository<Domain.Aggregates.MediaItem>();

        var mediaItems = await readOnlyRepository.GetAll(x => keys.Contains(x.Id));

        return mediaItems.ToDictionary(x => x.Id);
    }
}