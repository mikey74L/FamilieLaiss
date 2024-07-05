using DomainHelper.Interfaces;
using FLBackEnd.API.Enums;
using HotChocolate.Subscriptions;
using MediatR;

namespace FLBackEnd.API.Mediator.Commands.VideoConvertStatus;

/// <summary>
/// Mediatr Command for video convert status progress
/// </summary>
public class MtrVideoConvertStatusProgressCmd : IRequest
{
    /// <summary>
    /// ID of upload video
    /// </summary>
    public required long UploadVideoId { get; init; }

    /// <summary>
    /// ID of video convert status
    /// </summary>
    public required long VideoConvertStatusId { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for video convert status progress
/// </summary>
public class MtrVideoConvertStatusProgressCmdHandler(
    iUnitOfWork unitOfWork,
    ITopicEventSender eventSender,
    ILogger<MtrVideoConvertStatusProgressCmdHandler> logger) : IRequestHandler<MtrVideoConvertStatusProgressCmd>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task Handle(MtrVideoConvertStatusProgressCmd request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Mediatr-Handler for video convert status progress was called with parameter: {$Request}",
            request);

        logger.LogDebug("Get repository for VideoConvertStatus");
        var repositoryStatus = unitOfWork.GetRepository<Domain.Entities.VideoConvertStatus>();

        logger.LogDebug($"Get entity with id = {request.VideoConvertStatusId} from data store");
        var itemToSignal = await repositoryStatus.GetOneAsync(request.VideoConvertStatusId,
            nameof(Domain.Entities.VideoConvertStatus.UploadVideo));
        if (itemToSignal == null)
        {
            return;
        }

        logger.LogDebug("Sending event to subscribers");
        await eventSender.SendAsync(nameof(SubscriptionType.VideoConvertStatusCurrent), itemToSignal,
            cancellationToken);
    }
}