using DomainHelper.Interfaces;
using GreenDonut;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace User.API.GraphQL.DataLoaders.UserAccount;

public class UserAccountDataLoader(
    iUnitOfWork unitOfWork,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null) : BatchDataLoader<string, User.Domain.Aggregates.UserAccount>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<string, User.Domain.Aggregates.UserAccount>> LoadBatchAsync(
        IReadOnlyList<string> keys, CancellationToken cancellationToken)
    {
        var readOnlyRepository = unitOfWork.GetReadOnlyRepository<User.Domain.Aggregates.UserAccount>();

        var userAccountItems = await readOnlyRepository.GetAll(x => keys.Contains(x.Id));

        return userAccountItems.ToDictionary(x => x.Id);
    }
}