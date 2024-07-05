using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using FLBackEnd.API.Enums;
using FLBackEnd.API.Interfaces;
using Google.DTO;
using HotChocolate.Subscriptions;
using MediatR;

namespace FLBackEnd.API.Mediator.Commands.UploadPicture;

/// <summary>
/// Mediatr Command for set status for picture to converted
/// </summary>
public class MtrSetPictureStateConvertedCmd : IRequest
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
/// Mediatr Command-Handler for set status for picture to converted
/// </summary>
public class MtrSetPictureStateConvertedCmdHandler(
    iUnitOfWork unitOfWork,
    IGoogleMicroService googleMicroService,
    ITopicEventSender eventSender,
    ILogger<MtrSetPictureStateConvertedCmdHandler> logger) : IRequestHandler<MtrSetPictureStateConvertedCmd>
{
    #region Mediatr-Handler

    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task Handle(MtrSetPictureStateConvertedCmd request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Mediatr-Handler for set picture state to converted command was called with parameter: {$Request}",
            request);

        logger.LogDebug("Get repository for UploadPicture");
        var repository = unitOfWork.GetRepository<Domain.Entities.UploadPicture>();

        logger.LogDebug($"Get upload picture domain model with id = {request.UploadPictureId} from data store");
        var itemToChange = await repository.GetOneAsync(request.UploadPictureId);
        if (itemToChange == null)
        {
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Could not find upload picture with id = {request.UploadPictureId}");
        }

        logger.LogDebug("Updating domain model");
        itemToChange.SetPictureStateToConverted();

        if (itemToChange.UploadPictureExifInfo != null)
        {
            if (itemToChange.UploadPictureExifInfo.GpsLongitude.HasValue &&
                itemToChange.UploadPictureExifInfo.GpsLatitude.HasValue)
            {
                logger.LogDebug("Making request to google api service");
                var requestGoogle = new GoogleGeoCodingRequestDTO()
                {
                    Latitude = itemToChange.UploadPictureExifInfo.GpsLatitude.Value,
                    Longitude = itemToChange.UploadPictureExifInfo.GpsLongitude.Value
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
                logger.LogDebug("Geo-Coding-Address not needed because of no GPS-Data in Exif-Data");
            }
        }
        else
        {
            logger.LogDebug("Geo-Coding-Address not needed because of no Exif-Data was found");
        }

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();

        logger.LogDebug("Get repository for PictureConvertStatus");
        var repositoryStatus = unitOfWork.GetRepository<Domain.Entities.PictureConvertStatus>();

        logger.LogDebug($"Get picture convert status item with id = {request.PictureConvertStatusId} from data store");
        var itemToSignal = await repositoryStatus.GetOneAsync(request.PictureConvertStatusId,
            nameof(Domain.Entities.PictureConvertStatus.UploadPicture));

        logger.LogDebug("Sending event to subscribers");
        await eventSender.SendAsync(nameof(SubscriptionType.PictureConvertStatusSuccess), itemToSignal,
            cancellationToken);
    }

    #endregion
}