using DomainHelper.Interfaces;
using FamilieLaissMassTransitDefinitions.Contracts.Events;
using Google.DTO;
using MediatR;
using Upload.API.Interfaces;

namespace Upload.API.Mediator.Commands.UploadPicture;

/// <summary>
/// Mediatr Command for set picture state to converted
/// </summary>
public class MtrSetPictureStateConvertedCmd : IRequest
{
    /// <summary>
    /// Message data
    /// </summary>
    public required IMassPictureConvertedEvent Message { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for set picture state to converted
/// </summary>
/// <remarks>
/// Primary constructor 
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrSetPictureStateConvertedCmdHandler(
    iUnitOfWork unitOfWork,
    IGoogleMicroService googleMicroService,
    ILogger<MtrSetPictureStateConvertedCmdHandler> logger)
    : IRequestHandler<MtrSetPictureStateConvertedCmd>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task Handle(MtrSetPictureStateConvertedCmd request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for set picture state to converted command was called for {@Message}",
            request.Message);

        logger.LogInformation("Get repository from unit of work");
        var repo = unitOfWork.GetRepository<Domain.Entities.UploadPicture>();

        logger.LogInformation($"Get entity from repository for ID = {request.Message.UploadPictureId}");
        var entity = await repo.GetOneAsync(request.Message.UploadPictureId);

        logger.LogInformation($"Set state to converted");
        entity.SetPictureStateToConverted();

        if (entity.UploadPictureExifInfo != null)
        {
            if (entity.UploadPictureExifInfo.GpsLongitude.HasValue &&
                entity.UploadPictureExifInfo.GpsLatitude.HasValue)
            {
                logger.LogDebug("Making request to google api service");
                var requestGoogle = new GoogleGeoCodingRequestDTO()
                {
                    Latitude = entity.UploadPictureExifInfo.GpsLatitude.Value,
                    Longitude = entity.UploadPictureExifInfo.GpsLongitude.Value
                };
                var receiveData = await googleMicroService.GetGoogleGeoCodingAddressAsync(requestGoogle);

                if (receiveData is not null)
                {
                    logger.LogDebug("Setting Geo-Coding-Data on entity");
                    entity.SetGeoCodingAddress(receiveData.Longitude, receiveData.Latitude,
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

        logger.LogInformation("Save changes");
        await unitOfWork.SaveChangesAsync();
    }
}