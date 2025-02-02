using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DomainHelper.Interfaces;
using GreenDonut;

namespace UserInteraction.API.GraphQl.DataLoaders.Rating;

public class RatingDataLoader(
    iUnitOfWork unitOfWork,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null) : BatchDataLoader<long, Domain.Aggregates.Rating>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<long, Domain.Aggregates.Rating>> LoadBatchAsync(
        IReadOnlyList<long> keys, CancellationToken cancellationToken)
    {
        var readOnlyRepository = unitOfWork.GetReadOnlyRepository<Domain.Aggregates.Rating>();

        var ratingItems = await readOnlyRepository.GetAll(x => keys.Contains(x.Id));

        return ratingItems.ToDictionary(x => x.Id);
    }
}