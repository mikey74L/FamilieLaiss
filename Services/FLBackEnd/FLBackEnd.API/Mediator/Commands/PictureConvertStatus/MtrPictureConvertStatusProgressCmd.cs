using DomainHelper.Interfaces;
using FLBackEnd.API.Enums;
using HotChocolate.Subscriptions;
using MediatR;

namespace FLBackEnd.API.Mediator.Commands.PictureConvertStatus;

/// <summary>
/// Mediatr Command for picture convert status progress
/// </summary>
public class MtrPictureConvertStatusProgressCmd : IRequest
{
    /// <summary>
    /// ID of upload picture
    /// </summary>
    public required long UploadPictureId { get; init; }

    /// <summary>
    /// ID of picture convert status
    /// </summary>
    public required long PictureConvertStatusId { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for picture convert status progress
/// </summary>
public class MtrPictureConvertStatusProgressCmdHandler(
    iUnitOfWork unitOfWork,
    ITopicEventSender eventSender,
    ILogger<MtrPictureConvertStatusProgressCmdHandler> logger) : IRequestHandler<MtrPictureConvertStatusProgressCmd>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task Handle(MtrPictureConvertStatusProgressCmd request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Mediatr-Handler for picture convert status progress was called with parameter: {$Request}",
            request);

        logger.LogDebug("Get repository for PictureConvertStatus");
        var repositoryStatus = unitOfWork.GetRepository<Domain.Entities.PictureConvertStatus>();

        logger.LogDebug($"Get entity with id = {request.PictureConvertStatusId} from data store");
        var itemToSignal = await repositoryStatus.GetOneAsync(request.PictureConvertStatusId,
            nameof(Domain.Entities.PictureConvertStatus.UploadPicture));
        if (itemToSignal == null)
        {
            return;
        }

        logger.LogDebug("Sending event to subscribers");
        await eventSender.SendAsync(nameof(SubscriptionType.PictureConvertStatusCurrent), itemToSignal,
            cancellationToken);
    }
}