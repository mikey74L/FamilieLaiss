using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DomainHelper.Interfaces;
using GreenDonut;

namespace UserInteraction.API.GraphQl.DataLoaders.UserInteraction;

public class UserInteractionDataLoader(
    iUnitOfWork unitOfWork,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null) : BatchDataLoader<long, Domain.Aggregates.UserInteractionInfo>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<long, global::UserInteraction.Domain.Aggregates.UserInteractionInfo>> LoadBatchAsync(
        IReadOnlyList<long> keys, CancellationToken cancellationToken)
    {
        var readOnlyRepository = unitOfWork.GetReadOnlyRepository<global::UserInteraction.Domain.Aggregates.UserInteractionInfo>();

        var userInteractionItems = await readOnlyRepository.GetAll(x => keys.Contains(x.Id));

        return userInteractionItems.ToDictionary(x => x.Id);
    }
}