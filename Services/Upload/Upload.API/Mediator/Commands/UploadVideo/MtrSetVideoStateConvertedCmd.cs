using DomainHelper.Interfaces;
using FamilieLaissMassTransitDefinitions.Contracts.Events;
using Google.DTO;
using MediatR;
using Upload.API.Interfaces;

namespace Upload.API.Mediator.Commands.UploadVideo;

/// <summary>
/// Mediatr Command for set video state to converted
/// </summary>
public class MtrSetVideoStateConvertedCmd : IRequest
{
    /// <summary>
    /// Message data
    /// </summary>
    public required IMassVideoConvertedEvent Message { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for set video state to converted
/// </summary>
/// <remarks>
/// Primary constructor 
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrSetVideoStateConvertedCmdHandler(
    iUnitOfWork unitOfWork,
    IGoogleMicroService googleMicroService,
    ILogger<MtrSetVideoStateConvertedCmdHandler> logger)
    : IRequestHandler<MtrSetVideoStateConvertedCmd>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task Handle(MtrSetVideoStateConvertedCmd request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for set video state to converted command was called for {@Message}",
            request.Message);

        logger.LogInformation("Get repository from unit of work");
        var repo = unitOfWork.GetRepository<Domain.Entities.UploadVideo>();

        logger.LogInformation($"Get entity from repository for ID = {request.Message.UploadVideoId}");
        var entity = await repo.GetOneAsync(request.Message.UploadVideoId);

        logger.LogInformation($"Set state to converted");
        entity.SetVideoStateToConverted();

        if (entity is { GpsLongitude: not null, GpsLatitude: not null })
        {
            logger.LogDebug("Getting Google-Geo-Data from Google-API");

            logger.LogDebug("Making request to google api service");
            var requestGoogle = new GoogleGeoCodingRequestDTO()
            {
                Latitude = entity.GpsLatitude.Value,
                Longitude = entity.GpsLongitude.Value
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
            logger.LogDebug("Geo-Coding-Address not needed because of no GPS-Data found for video");
        }

        logger.LogInformation("Save changes");
        await unitOfWork.SaveChangesAsync();
    }
}