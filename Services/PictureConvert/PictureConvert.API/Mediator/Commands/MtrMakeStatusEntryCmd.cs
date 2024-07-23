using DomainHelper.Interfaces;
using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadPicture;
using MediatR;
using PictureConvert.Domain.Entities;

namespace PictureConvert.API.Mediator.Commands;

/// <summary>
/// Mediatr Command for Make Status Entry
/// </summary>
public class MtrMakeStatusEntryCmd : IRequest
{
    #region Public Properties

    public required IMassPictureUploadedEvent Data { get; init; }

    #endregion
}

/// <summary>
/// Mediatr Command-Handler for Make Status Entry command
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrMakeStatusEntryCmdHandler(
    iUnitOfWork unitOfWork,
    ILogger<MtrMakeStatusEntryCmdHandler> logger) : IRequestHandler<MtrMakeStatusEntryCmd>
{
    #region Mediatr-Handler

    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task Handle(MtrMakeStatusEntryCmd request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for Make Status Entry command was called: {Data}", request.Data);

        logger.LogDebug("Get repository for UploadPicture");
        var repositoryPicture = unitOfWork.GetRepository<UploadPicture>();

        logger.LogDebug("Adding new UploadPicture domain model to repository");
        UploadPicture newPicture = new(request.Data.Id, request.Data.Filename);
        await repositoryPicture.AddAsync(newPicture);

        logger.LogDebug("Get repository for ConvertStatus");
        var repositoryStatus = unitOfWork.GetRepository<PictureConvertStatus>();

        logger.LogDebug("Adding new ConvertStatus domain model to repository");
        PictureConvertStatus newStatus = new(newPicture);
        await repositoryStatus.AddAsync(newStatus);

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();
    }

    #endregion
}