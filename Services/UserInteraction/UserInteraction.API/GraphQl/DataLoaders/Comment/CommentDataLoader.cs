using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DomainHelper.Interfaces;
using GreenDonut;

namespace UserInteraction.API.GraphQl.DataLoaders.Comment;

public class CommentDataLoader(
    iUnitOfWork unitOfWork,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null) : BatchDataLoader<long, Domain.Aggregates.Comment>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<long, Domain.Aggregates.Comment>> LoadBatchAsync(
        IReadOnlyList<long> keys, CancellationToken cancellationToken)
    {
        var readOnlyRepository = unitOfWork.GetReadOnlyRepository<Domain.Aggregates.Comment>();

        var commentItems = await readOnlyRepository.GetAll(x => keys.Contains(x.Id));

        return commentItems.ToDictionary(x => x.Id);
    }
}