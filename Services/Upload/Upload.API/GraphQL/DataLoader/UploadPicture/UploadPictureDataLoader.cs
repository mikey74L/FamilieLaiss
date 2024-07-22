using DomainHelper.Interfaces;

namespace Upload.API.GraphQL.DataLoader.UploadPicture;

public class UploadPictureDataLoader(
    iUnitOfWork unitOfWork,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null) : BatchDataLoader<long, Domain.Entities.UploadPicture>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<long, Domain.Entities.UploadPicture>> LoadBatchAsync(
        IReadOnlyList<long> keys, CancellationToken cancellationToken)
    {
        var readOnlyRepository = unitOfWork.GetReadOnlyRepository<Domain.Entities.UploadPicture>();

        var uploadPictures = await readOnlyRepository.GetAll(x => keys.Contains(x.Id));

        return uploadPictures.ToDictionary(x => x.Id);
    }
}