using DomainHelper.Interfaces;
using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadVideo;
using MediatR;
using VideoConvert.Domain.Entities;

namespace VideoConvert.API.Mediator.Commands;

/// <summary>
/// Mediatr Command for delete status entry
/// </summary>
public class MtrDeleteStatusEntryCmd : IRequest
{
    #region Public Properties

    /// <summary>
    /// Data for Event
    /// </summary>
    public required IMassUploadVideoDeletedEvent Data { get; init; }

    #endregion
}

/// <summary>
/// Mediatr Command-Handler for delete status entry command
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrDeleteStatusEntryCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrDeleteStatusEntryCmdHandler> logger)
    : IRequestHandler<MtrDeleteStatusEntryCmd>
{
    #region Mediatr-Handler

    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task Handle(MtrDeleteStatusEntryCmd request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for delete status entry command was called: {Data}", request.Data);

        logger.LogDebug("Get repository for ConvertStatus");
        var repositoryStatus = unitOfWork.GetRepository<VideoConvertStatus>();

        logger.LogDebug("Get status item from repository");
        var itemDoDelete =
            (await repositoryStatus.GetAll(x => x.UploadVideoId == request.Data.Id, null, "UploadVideo"))
            .FirstOrDefault();

        if (itemDoDelete is not null)
        {
            logger.LogDebug("Remove status item from repository");
            repositoryStatus.Delete(itemDoDelete);

            logger.LogDebug("Saving changes to data store");
            await unitOfWork.SaveChangesAsync();
        }
    }

    #endregion
}