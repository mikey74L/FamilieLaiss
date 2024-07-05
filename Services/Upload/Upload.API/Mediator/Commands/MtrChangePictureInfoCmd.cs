using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using MediatR;
using Upload.Domain.Entities;

namespace Upload.API.Mediator.Commands
{
    /// <summary>
    /// Mediatr Command for Change the picture info
    /// </summary>
    public class MtrChangePictureInfoCmd : IRequest
    {
        #region Public Properties
        /// <summary>
        /// The ID for the upload picture
        /// </summary>
        public long ID { get; private set; }

        /// <summary>
        /// The height in pixels for the upload picture
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// The width in pixels for the upload picture
        /// </summary>
        public int Width { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">The ID for the upload picture</param>
        /// <param name="height">The height in Pixels for the upload picture</param>
        /// <param name="width">The width in Pixels for the upload picture</param>
        public MtrChangePictureInfoCmd(long id, int height, int width)
        {
            //Übernehmen der Parameter in die Properties
            ID = id;
            Height = height;
            Width = width;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for Change picture info command
    /// </summary>
    public class MtrChangePictureInfoCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrChangePictureInfoCmdHandler> logger) : IRequestHandler<MtrChangePictureInfoCmd>
    {
        #region Mediatr-Handler
        /// <summary>
        /// Will be called by Mediatr
        /// </summary>
        /// <param name="request">The request data</param>
        /// <param name="cancellationToken">The cancelation token</param>
        /// <returns>Task</returns>
        public async Task Handle(MtrChangePictureInfoCmd request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Mediatr-Handler for Change picture info command was called with following parameters: {$Input}", request);

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
            itemToChange.UpdateSize(request.Height, request.Width);
            repository.Update(itemToChange);

            //Speichern der Daten
            logger.LogDebug("Saving changes to data store");
            await unitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}
