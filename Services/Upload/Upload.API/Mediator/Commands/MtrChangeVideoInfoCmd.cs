using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using FamilieLaissSharedObjects.Enums;
using MediatR;
using Upload.Domain.Entities;

namespace Upload.API.Mediator.Commands
{
    /// <summary>
    /// Mediatr Command for Change the video info
    /// </summary>
    public class MtrChangeVideoInfoCmd : IRequest
    {
        #region Public Properties
        /// <summary>
        /// Id for the upload video
        /// </summary>
        public long ID { get; init; }

        /// <summary>
        /// Type of video
        /// </summary>
        public EnumVideoType VideoType { get; init; }

        /// <summary>
        /// Height of the original video
        /// </summary>
        public int Height { get; init; }

        /// <summary>
        /// Width of the original video
        /// </summary>
        public int Width { get; init; }

        /// <summary>
        /// The hour part for the duration of this video
        /// </summary>
        public int Duration_Hour { get; init; }

        /// <summary>
        /// The minute part for the duration of this video
        /// </summary>
        public int Duration_Minute { get; init; }

        /// <summary>
        /// The second part for the duration of this video
        /// </summary>
        public int Duration_Second { get; init; }

        /// <summary>
        /// Longitude for GPS-Position
        /// </summary>
        public double? GPS_Longitude { get; init; }

        /// <summary>
        /// Latitude for GPS-Position
        /// </summary>
        public double? GPS_Latitude { get; init; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">The ID for the upload video</param>
        /// <param name="videoType">Type of video</param>
        /// <param name="height">Height of the original video</param>
        /// <param name="width">Width of the original video</param>
        /// <param name="durationHour">The hour part for the duration of this video</param>
        /// <param name="durationMinute">The minute part for the duration of this video</param>
        /// <param name="durationSecond">The second part for the duration of this video</param>
        /// <param name="latitude">Latitude for GPS-Position</param>
        /// <param name="longitude">Longitude for GPS-Position</param>
        public MtrChangeVideoInfoCmd(long id, EnumVideoType videoType, int height, int width, int durationHour, int durationMinute,
            int durationSecond, double? longitude, double? latitude)
        {
            //Übernehmen der Parameter in die Properties
            ID = id;
            VideoType = videoType;
            Height = height;
            Width = width;
            Duration_Hour = durationHour;
            Duration_Minute = durationMinute;
            Duration_Second = durationSecond;
            GPS_Longitude = longitude;
            GPS_Latitude = latitude;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for Change video info command
    /// </summary>
    public class MtrChangeVideoInfoCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrChangeVideoInfoCmdHandler> logger) : IRequestHandler<MtrChangeVideoInfoCmd>
    {
        #region Mediatr-Handler
        /// <summary>
        /// Will be called by Mediatr
        /// </summary>
        /// <param name="request">The request data</param>
        /// <param name="cancellationToken">The cancelation token</param>
        /// <returns>Task</returns>
        public async Task Handle(MtrChangeVideoInfoCmd request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Mediatr-Handler for Change video info command was called with following parameters: {$Input}", request);

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
            itemToChange.UpdateVideoInfo(request.VideoType, request.Height, request.Width, request.Duration_Hour, request.Duration_Minute,
                request.Duration_Second, request.GPS_Longitude, request.GPS_Latitude);
            repository.Update(itemToChange);

            //Speichern der Daten
            logger.LogDebug("Saving changes to data store");
            await unitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}
