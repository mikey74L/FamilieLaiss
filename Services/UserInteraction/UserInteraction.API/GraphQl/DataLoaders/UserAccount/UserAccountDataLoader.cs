using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DomainHelper.Interfaces;
using GreenDonut;

namespace UserInteraction.API.GraphQl.DataLoaders.UserAccount;

public class UserAccountDataLoader(
    iUnitOfWork unitOfWork,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null) : BatchDataLoader<string, Domain.Aggregates.UserAccount>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<string, Domain.Aggregates.UserAccount>> LoadBatchAsync(
        IReadOnlyList<string> keys, CancellationToken cancellationToken)
    {
        var readOnlyRepository = unitOfWork.GetReadOnlyRepository<Domain.Aggregates.UserAccount>();

        var userAccountItems = await readOnlyRepository.GetAll(x => keys.Contains(x.Id));

        return userAccountItems.ToDictionary(x => x.Id);
    }
}