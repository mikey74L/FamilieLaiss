using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using FLBackEnd.API.Enums;
using FLBackEnd.API.Interfaces;
using Google.DTO;
using HotChocolate.Subscriptions;
using MediatR;

namespace FLBackEnd.API.Mediator.Commands.UploadVideo;

/// <summary>
/// Mediatr Command for set status for video to converted
/// </summary>
public class MtrSetVideoStateConvertedCmd : IRequest
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
/// Mediatr Command-Handler for set status for video to converted
/// </summary>
/// <remarks>
/// Primary constructor
/// </remarks>
/// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
/// <param name="googleMicroService">The proxy for calling google api microservice. Will be injected by DI-Container</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrSetVideoStateConvertedCmdHandler(
    iUnitOfWork unitOfWork,
    IGoogleMicroService googleMicroService,
    ITopicEventSender eventSender,
    ILogger<MtrSetVideoStateConvertedCmdHandler> logger) : IRequestHandler<MtrSetVideoStateConvertedCmd>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task Handle(MtrSetVideoStateConvertedCmd request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Mediatr-Handler for set video state to converted command was called with following parameters");
        logger.LogDebug($"ID: {request.UploadVideoId}");

        logger.LogDebug("Get repository for UploadVideo");
        var repository = unitOfWork.GetRepository<Domain.Entities.UploadVideo>();

        logger.LogDebug($"Get upload video domain model with id = {request.UploadVideoId} from data store");
        var itemToChange = await repository.GetOneAsync(request.UploadVideoId);
        if (itemToChange == null)
        {
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Could not find upload video with id = {request.UploadVideoId}");
        }

        logger.LogDebug("Updating domain model");
        itemToChange.SetVideoStateToConverted();

        if (itemToChange is { GpsLongitude: not null, GpsLatitude: not null })
        {
            logger.LogDebug("Getting Google-Geo-Data from Google-API");

            logger.LogDebug("Making request to google api service");
            var requestGoogle = new GoogleGeoCodingRequestDTO()
            {
                Latitude = itemToChange.GpsLatitude.Value,
                Longitude = itemToChange.GpsLongitude.Value
            };
            var receiveData = await googleMicroService.GetGoogleGeoCodingAddressAsync(requestGoogle);

            if (receiveData is not null)
            {
                logger.LogDebug("Setting Geo-Coding-Data on entity");
                itemToChange.SetGeoCodingAddress(receiveData.Longitude, receiveData.Latitude,
                    receiveData.StreetName,
                    receiveData.HNR, receiveData.ZIP, receiveData.City, receiveData.Country);
            }
            else
            {
                logger.LogError("Error while fetching Geo-Coding-Data from Google");
            }
        }
        else
        {
            logger.LogDebug("Geo-Coding-Address not needed because of no GPS-Data found for video");
        }

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();

        logger.LogDebug("Get repository for VideoConvertStatus");
        var repositoryStatus = unitOfWork.GetRepository<Domain.Entities.VideoConvertStatus>();

        logger.LogDebug($"Get video convert status item with id = {request.VideoConvertStatusId} from data store");
        var itemToSignal = await repositoryStatus.GetOneAsync(request.VideoConvertStatusId,
            nameof(Domain.Entities.VideoConvertStatus.UploadVideo));

        logger.LogDebug("Sending event to subscribers");
        await eventSender.SendAsync(nameof(SubscriptionType.VideoConvertStatusSuccess), itemToSignal,
            cancellationToken);
    }
}