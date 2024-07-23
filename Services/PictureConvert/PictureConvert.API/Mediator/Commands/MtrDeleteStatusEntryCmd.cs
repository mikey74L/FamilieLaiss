using DomainHelper.Interfaces;
using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadPicture;
using MediatR;
using PictureConvert.Domain.Entities;

namespace PictureConvert.API.Mediator.Commands;

/// <summary>
/// Mediatr Command for Delete Status Entry
/// </summary>
public class MtrDeleteStatusEntryCmd : IRequest
{
    #region Public Properties

    /// <summary>
    /// ID for upload picture
    /// </summary>
    public required IMassUploadPictureDeletedEvent Data { get; init; }

    #endregion
}

/// <summary>
/// Mediatr Command-Handler for Delete status entry command
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
        logger.LogInformation("Mediatr-Handler for Delete Status Entry command was called: {Data}", request.Data);

        logger.LogDebug("Get repository for ConvertStatus");
        iRepository<PictureConvertStatus> repositoryStatus = unitOfWork.GetRepository<PictureConvertStatus>();

        logger.LogDebug("Get status item from repository");
        var itemDoDelete =
            (await repositoryStatus.GetAll(x => x.UploadPictureId == request.Data.Id, null, "UploadPicture"))
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