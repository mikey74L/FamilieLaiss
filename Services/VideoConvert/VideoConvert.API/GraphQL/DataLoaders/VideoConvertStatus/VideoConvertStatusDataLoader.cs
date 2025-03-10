using DomainHelper.Interfaces;

namespace PictureConvert.API.GraphQL.DataLoader.VideoConvertStatus;

public class VideoConvertStatusDataLoader(
    iUnitOfWork unitOfWork,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null) : BatchDataLoader<long, VideoConvert.Domain.Entities.VideoConvertStatus>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<long, VideoConvert.Domain.Entities.VideoConvertStatus>> LoadBatchAsync(
        IReadOnlyList<long> keys, CancellationToken cancellationToken)
    {
        var readOnlyRepository = unitOfWork.GetReadOnlyRepository<VideoConvert.Domain.Entities.VideoConvertStatus>();

        var videoConvertStatusItems = await readOnlyRepository.GetAll(x => keys.Contains(x.Id));

        return videoConvertStatusItems.ToDictionary(x => x.Id);
    }
}