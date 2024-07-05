using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using Google.DTO;
using MediatR;
using Upload.API.Interfaces;
using Upload.Domain.Entities;

namespace Upload.API.Mediator.Commands
{
    /// <summary>
    /// Mediatr Command for set status for video to converted
    /// </summary>
    public class MtrSetVideoStateConvertedCmd : IRequest
    {
        #region Public Properties
        /// <summary>
        /// ID of upload picture
        /// </summary>
        public required long ID { get; init; }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for set status for video to converted
    /// </summary>
    public class MtrSetVideoStateConvertedCmdHandler(iUnitOfWork unitOfWork, IGoogleMicroService googleMicroService,
            ILogger<MtrSetVideoStateConvertedCmdHandler> logger) : IRequestHandler<MtrSetVideoStateConvertedCmd>
    {
        #region Mediatr-Handler
        /// <summary>
        /// Will be called by Mediatr
        /// </summary>
        /// <param name="request">The request data</param>
        /// <param name="cancellationToken">The cancelation token</param>
        /// <returns>Task</returns>
        public async Task Handle(MtrSetVideoStateConvertedCmd request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Mediatr-Handler for set video state to converted command was called with ID = {request.ID}");

            //Repository ermitteln
            logger.LogDebug("Get repository for UploadVideo");
            var repository = unitOfWork.GetRepository<UploadVideo>();

            //Ermitteln des zu ändernden Items aus dem Store
            logger.LogDebug($"Get upload video domain model with id = {request.ID} from data store");
            var itemToChange = await repository.GetOneAsync(request.ID);
            if (itemToChange == null)
            {
                throw new DomainException(DomainExceptionType.NoDataFound, $"Could not find upload video with id = {request.ID}");
            }

            //Anpassen der Daten
            logger.LogDebug("Updating domain model");
            itemToChange.SetVideoStateToConverted();

            //Setzen der Geo-Coding-Daten in der Entity
            if (itemToChange.GpsLongitude.HasValue && itemToChange.GpsLatitude.HasValue)
            {
                //Eine Retry-Policy mit Polly erstellen, da es sein kann das die Google-API kurzzeitig nicht erreichbar ist
                logger.LogDebug("Getting Google-Geo-Data from Google-API");

                //Absetzen des Request an den Google-API-Service
                logger.LogDebug("Making request to google api service");
                var requestGoogle = new GoogleGeoCodingRequestDTO()
                {
                    Latitude = itemToChange.GpsLatitude.Value,
                    Longitude = itemToChange.GpsLongitude.Value
                };
                var receiveData = await googleMicroService.GetGoogleGeoCodingAdressAsync(requestGoogle);

                if (receiveData is not null)
                {
                    //Setzen der Receive-Daten für die Entity
                    logger.LogDebug("Setting Geo-Coding-Data on entity");
                    itemToChange.SetGeoCodingAdress(receiveData.Longitude, receiveData.Latitude, receiveData.StreetName,
                        receiveData.HNR, receiveData.ZIP, receiveData.City, receiveData.Country);
                }
                else
                {
                    logger.LogError("Error while fetching Geo-Coding-Data from Google");
                }
            }
            else
            {
                logger.LogDebug("Geo-Coding-Adress not needed because of no GPS-Data found for video");
            }

            //Speichern der Daten
            logger.LogDebug("Saving changes to data store");
            await unitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}
