using DomainHelper.Interfaces;

namespace Upload.API.GraphQL.DataLoader.UploadVideo;

public class UploadVideoDataLoader(
    iUnitOfWork unitOfWork,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null) : BatchDataLoader<long, Domain.Entities.UploadVideo>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<long, Domain.Entities.UploadVideo>> LoadBatchAsync(
        IReadOnlyList<long> keys, CancellationToken cancellationToken)
    {
        var readOnlyRepository = unitOfWork.GetReadOnlyRepository<Domain.Entities.UploadVideo>();

        var uploadVideos = await readOnlyRepository.GetAll(x => keys.Contains(x.Id));

        return uploadVideos.ToDictionary(x => x.Id);
    }
}