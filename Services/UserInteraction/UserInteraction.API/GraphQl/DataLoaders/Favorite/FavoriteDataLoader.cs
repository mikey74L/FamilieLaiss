using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DomainHelper.Interfaces;
using GreenDonut;

namespace UserInteraction.API.GraphQl.DataLoaders.Favorite;

public class FavoriteDataLoader(
    iUnitOfWork unitOfWork,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null) : BatchDataLoader<long, Domain.Aggregates.Favorite>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<long, Domain.Aggregates.Favorite>> LoadBatchAsync(
        IReadOnlyList<long> keys, CancellationToken cancellationToken)
    {
        var readOnlyRepository = unitOfWork.GetReadOnlyRepository<Domain.Aggregates.Favorite>();

        var favoriteItems = await readOnlyRepository.GetAll(x => keys.Contains(x.Id));

        return favoriteItems.ToDictionary(x => x.Id);
    }
}