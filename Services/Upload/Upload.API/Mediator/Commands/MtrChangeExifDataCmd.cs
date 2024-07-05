using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using MediatR;
using Upload.Domain.Entities;

namespace Upload.API.Mediator.Commands
{
    /// <summary>
    /// Mediatr Command for Change the Exif data
    /// </summary>
    public class MtrChangeExifDataCmd : IRequest
    {
        #region Public Properties
        /// <summary>
        /// ID of upload picture
        /// </summary>
        public long ID { get; }

        /// <summary>
        /// Make for the camera
        /// </summary>
        public string Make { get; }

        /// <summary>
        /// Camera model
        /// </summary>
        public string Model { get; }

        /// <summary>
        /// Resolution X
        /// </summary>
        public double? Resolution_X { get; }

        /// <summary>
        /// Resolution Y
        /// </summary>
        public double? Resolution_Y { get; }

        /// <summary>
        /// The resolution unit
        /// </summary>
        public string Resolution_Unit { get; }

        /// <summary>
        /// Orientation for picture
        /// </summary>
        public short? Orientation { get; }

        /// <summary>
        /// Timestamp when picture was recorded
        /// </summary>
        public DateTimeOffset? DDL_Recorded { get; }

        /// <summary>
        /// Exposure-Time
        /// </summary>
        public string Exposure_Time { get; }

        /// <summary>
        /// Exposure program
        /// </summary>
        public short? Exposure_Program { get; }

        /// <summary>
        /// Exposure mode
        /// </summary>
        public short? Exposure_Mode { get; }

        /// <summary>
        /// F-Number
        /// </summary>
        public double? F_Number { get; }

        /// <summary>
        /// ISO sensitivity
        /// </summary>
        public int? ISO_Sensitivity { get; }

        /// <summary>
        /// Shutter speed
        /// </summary>
        public double? Shutter_Speed { get; }

        /// <summary>
        /// Metering mode
        /// </summary>
        public short? Metering_Mode { get; }

        /// <summary>
        /// Flash mode
        /// </summary>
        public short? Flash_Mode { get; }

        /// <summary>
        /// Focal length
        /// </summary>
        public double? Focal_Length { get; }

        /// <summary>
        /// Sensing mode
        /// </summary>
        public short? Sensing_Mode { get; }

        /// <summary>
        /// White-Balance mode
        /// </summary>
        public short? White_Balance_Mode { get; }

        /// <summary>
        /// Sharpness
        /// </summary>
        public short? Sharpness { get; }

        /// <summary>
        /// Longitutde for GPS-Position
        /// </summary>
        public double? GPS_Longitute { get; }

        /// <summary>
        /// Latitude for GPS-Position
        /// </summary>
        public double? GPS_Latitude { get; }

        /// <summary>
        /// Contrast
        /// </summary>
        public short? Contrast { get; }

        /// <summary>
        /// Saturation
        /// </summary>
        public short? Saturation { get; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">ID of upload picture</param>
        /// <param name="make">Make for the camera</param>
        /// <param name="model">Camera model</param>
        /// <param name="resolution_X">Resolution x</param>
        /// <param name="resolution_Y">Resolution Y</param>
        /// <param name="resolution_Unit">The resolution unit</param>
        /// <param name="orientation">Orientation for picture</param>
        /// <param name="ddl_Recorded">Timestamp when picture was recorded</param>
        /// <param name="exposure_Time">Exposure-Time</param>
        /// <param name="exposure_Program">Exposure program</param>
        /// <param name="exposure_Mode">Exposure mode</param>
        /// <param name="f_Number">F-Number</param>
        /// <param name="iso_Sensitivity">ISO sensitivity</param>
        /// <param name="shutter_Speed">Shutter speed</param>
        /// <param name="metering_Mode">Metering mode</param>
        /// <param name="flash_Mode">Flash mode</param>
        /// <param name="focal_Length">Focal length</param>
        /// <param name="sensing_Mode">Sensing mode</param>
        /// <param name="white_Balance_Mode">White-Balance mode</param>
        /// <param name="sharpness">Sharpness</param>
        /// <param name="gps_Longitute">Longitutde for GPS-Position</param>
        /// <param name="gps_Latitude">Latitude for GPS-Position</param>
        /// <param name="contrast">Contrast</param>
        /// <param name="saturation">Saturation</param>
        public MtrChangeExifDataCmd(long id, string make, string model, double? resolution_X, double? resolution_Y,
            string resolution_Unit, short? orientation, DateTimeOffset? ddl_Recorded, string exposure_Time,
            short? exposure_Program, short? exposure_Mode, double? f_Number,
            int? iso_Sensitivity, double? shutter_Speed, short? metering_Mode, short? flash_Mode, double? focal_Length,
            short? sensing_Mode, short? white_Balance_Mode, short? sharpness, double? gps_Longitute, double? gps_Latitude,
            short? contrast, short? saturation)
        {
            ID = id;
            Make = make;
            Model = model;
            Resolution_X = resolution_X;
            Resolution_Y = resolution_Y;
            Resolution_Unit = resolution_Unit;
            Orientation = orientation;
            DDL_Recorded = ddl_Recorded;
            Exposure_Time = exposure_Time;
            Exposure_Program = exposure_Program;
            Exposure_Mode = exposure_Mode;
            F_Number = f_Number;
            ISO_Sensitivity = iso_Sensitivity;
            Shutter_Speed = shutter_Speed;
            Metering_Mode = metering_Mode;
            Flash_Mode = flash_Mode;
            Focal_Length = focal_Length;
            Sensing_Mode = sensing_Mode;
            White_Balance_Mode = white_Balance_Mode;
            Sharpness = sharpness;
            GPS_Longitute = gps_Longitute;
            GPS_Latitude = gps_Latitude;
            Contrast = contrast;
            Saturation = saturation;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for Change the Exif data
    /// </summary>
    public class MtrChangeExifDataCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrChangeExifDataCmdHandler> logger) : IRequestHandler<MtrChangeExifDataCmd>
    {
        #region Mediatr-Handler
        /// <summary>
        /// Will be called by Mediatr
        /// </summary>
        /// <param name="request">The request data</param>
        /// <param name="cancellationToken">The cancelation token</param>
        /// <returns>Task</returns>
        public async Task Handle(MtrChangeExifDataCmd request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Mediatr-Handler for Change Exif data command was called with following parameters: {$Input}", request);

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
            itemToChange.SetExifData(request.Make, request.Model, request.Resolution_X, request.Resolution_Y, request.Resolution_Unit,
                request.Orientation, request.DDL_Recorded, request.Exposure_Time,
                request.Exposure_Program, request.Exposure_Mode, request.F_Number, request.ISO_Sensitivity, request.Shutter_Speed,
                request.Metering_Mode, request.Flash_Mode, request.Focal_Length, request.Sensing_Mode, request.White_Balance_Mode,
                request.Sharpness, request.GPS_Longitute, request.GPS_Latitude, request.Contrast, request.Saturation);
            repository.Update(itemToChange);

            //Speichern der Daten
            logger.LogDebug("Saving changes to data store");
            await unitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}
