using DomainHelper.Interfaces;
using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadVideo;
using MediatR;
using VideoConvert.Domain.Entities;

namespace VideoConvert.API.Mediator.Commands;

/// <summary>
/// Mediatr Command for make status entry
/// </summary>
public class MtrMakeStatusEntryCmd : IRequest
{
    #region Public Properties

    public required IMassVideoUploadedEvent Data { get; init; }

    #endregion
}

/// <summary>
/// Mediatr Command-Handler for make status entry command
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
        logger.LogInformation("Mediatr-Handler for make status entry command was called: {Data}", request.Data);

        logger.LogDebug("Get repository for UploadVideo");
        var repositoryVideo = unitOfWork.GetRepository<UploadVideo>();

        logger.LogDebug("Adding new UploadPicture domain model to repository");
        var newPicture = new UploadVideo(request.Data.Id, request.Data.Filename);
        await repositoryVideo.AddAsync(newPicture);

        logger.LogDebug("Get repository for ConvertStatus");
        var repositoryStatus = unitOfWork.GetRepository<VideoConvertStatus>();

        logger.LogDebug("Adding new ConvertStatus domain model to repository");
        var newStatus = new VideoConvertStatus(newPicture);
        await repositoryStatus.AddAsync(newStatus);

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();
    }

    #endregion
}