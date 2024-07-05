using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using Google.DTO;
using MediatR;
using Upload.API.Interfaces;
using Upload.Domain.Entities;

namespace Upload.API.Mediator.Commands
{
    /// <summary>
    /// Mediatr Command for set status for picture to converted
    /// </summary>
    public class MtrSetPictureStateConvertedCmd : IRequest
    {
        #region Public Properties
        /// <summary>
        /// ID of upload picture
        /// </summary>
        public required long ID { get; init; }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for set status for picture to converted
    /// </summary>
    public class MtrSetPictureStateConvertedCmdHandler(iUnitOfWork unitOfWork, IGoogleMicroService googleMicroService,
            ILogger<MtrSetPictureStateConvertedCmdHandler> logger) : IRequestHandler<MtrSetPictureStateConvertedCmd>
    {
        #region Mediatr-Handler
        /// <summary>
        /// Will be called by Mediatr
        /// </summary>
        /// <param name="request">The request data</param>
        /// <param name="cancellationToken">The cancelation token</param>
        /// <returns>Task</returns>
        public async Task Handle(MtrSetPictureStateConvertedCmd request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Mediatr-Handler for set picture state to converted command was called with ID = {request.ID}");

            //Repository ermitteln
            logger.LogDebug("Get repository for UploadPicture");
            var repository = unitOfWork.GetRepository<UploadPicture>();

            //Ermitteln des zu ändernden Items aus dem Store
            logger.LogDebug($"Get upload picture domain model with id = {request.ID} from data store");
            var itemToChange = await repository.GetOneAsync(request.ID);
            if (itemToChange == null)
            {
                throw new DomainException(DomainExceptionType.NoDataFound, $"Could not find upload picture with id = {request.ID}");
            }

            //Anpassen der Daten
            logger.LogDebug("Updating domain model");
            itemToChange.SetPictureStateToConverted();

            //Setzen der Geo-Coding-Daten in der Entity
            if (itemToChange.UploadPictureExifInfo != null)
            {
                if (itemToChange.UploadPictureExifInfo.GpsLongitude.HasValue && itemToChange.UploadPictureExifInfo.GpsLatitude.HasValue)
                {
                    //Absetzen des Request an den Google-API-Service
                    logger.LogDebug("Making request to google api service");
                    var requestGoogle = new GoogleGeoCodingRequestDTO()
                    {
                        Latitude = itemToChange.UploadPictureExifInfo.GpsLatitude.Value,
                        Longitude = itemToChange.UploadPictureExifInfo.GpsLongitude.Value
                    };
                    var receiveData = await googleMicroService.GetGoogleGeoCodingAdressAsync(requestGoogle);

                    //Setzen der Receive-Daten für die Entity
                    if (receiveData is not null)
                    {
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
                    logger.LogDebug("Geo-Coding-Adress not needed because of no GPS-Data in Exif-Data");
                }
            }
            else
            {
                logger.LogDebug("Geo-Coding-Adress not needed because of no Exif-Data was found");
            }

            //Speichern der Daten
            logger.LogDebug("Saving changes to data store");
            await unitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}