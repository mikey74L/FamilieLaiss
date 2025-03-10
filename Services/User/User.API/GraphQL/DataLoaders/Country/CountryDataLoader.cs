using DomainHelper.Interfaces;
using GreenDonut;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace User.API.GraphQL.DataLoaders.Country;

public class CountryDataLoader(
    iUnitOfWork unitOfWork,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null) : BatchDataLoader<string, User.Domain.Aggregates.Country>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<string, User.Domain.Aggregates.Country>> LoadBatchAsync(
        IReadOnlyList<string> keys, CancellationToken cancellationToken)
    {
        var readOnlyRepository = unitOfWork.GetReadOnlyRepository<User.Domain.Aggregates.Country>();

        var countryItems = await readOnlyRepository.GetAll(x => keys.Contains(x.Id));

        return countryItems.ToDictionary(x => x.Id);
    }
}