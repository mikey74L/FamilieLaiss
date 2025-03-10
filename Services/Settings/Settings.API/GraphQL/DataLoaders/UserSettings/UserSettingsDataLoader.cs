using DomainHelper.Interfaces;
using Settings.Domain.Entities;

namespace Settings.API.GraphQL.DataLoaders;

public class UserSettingsDataLoader(
    iUnitOfWork unitOfWork,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null) : BatchDataLoader<string, UserSetting>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<string, UserSetting>> LoadBatchAsync(
        IReadOnlyList<string> keys, CancellationToken cancellationToken)
    {
        var readOnlyRepository = unitOfWork.GetReadOnlyRepository<UserSetting>();

        var userSettingItems = await readOnlyRepository.GetAll(x => keys.Contains(x.Id));

        return userSettingItems.ToDictionary(x => x.Id);
    }
}