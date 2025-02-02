using DomainHelper.Interfaces;

namespace PictureConvert.API.GraphQL.DataLoaders.PictureConvertStatus;

public class PictureConvertStatusDataLoader(
    iUnitOfWork unitOfWork,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null) : BatchDataLoader<long, Domain.Entities.PictureConvertStatus>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<long, Domain.Entities.PictureConvertStatus>> LoadBatchAsync(
        IReadOnlyList<long> keys, CancellationToken cancellationToken)
    {
        var readOnlyRepository = unitOfWork.GetReadOnlyRepository<Domain.Entities.PictureConvertStatus>();

        var pictureConvertStatusItems = await readOnlyRepository.GetAll(x => keys.Contains(x.Id));

        return pictureConvertStatusItems.ToDictionary(x => x.Id);
    }
}