using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DomainHelper.Interfaces;
using GreenDonut;

namespace UserInteraction.API.GraphQl.DataLoaders.MediaItem;

public class MediaItemDataLoader(
    iUnitOfWork unitOfWork,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null) : BatchDataLoader<long, Domain.Aggregates.MediaItem>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<long, global::UserInteraction.Domain.Aggregates.MediaItem>> LoadBatchAsync(
        IReadOnlyList<long> keys, CancellationToken cancellationToken)
    {
        var readOnlyRepository = unitOfWork.GetReadOnlyRepository<global::UserInteraction.Domain.Aggregates.MediaItem>();

        var mediaItems = await readOnlyRepository.GetAll(x => keys.Contains(x.Id));

        return mediaItems.ToDictionary(x => x.Id);
    }
}