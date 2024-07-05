//using DomainHelper.Interfaces;
//using MediatR;

//namespace Catalog.API.Mediator.Commands.UploadPicture
//{
//    /// <summary>
//    /// Mediatr Command for Delete upload picture
//    /// </summary>
//    public class MtrDeleteUploadPictureCmd : IRequest
//    {
//        #region Public Properties
//        /// <summary>
//        /// ID for upload picture
//        /// </summary>
//        public long UploadID { get; private set; }
//        #endregion

//        #region C'tor
//        /// <summary>
//        /// C'tor
//        /// </summary>
//        /// <param name="uploadVideoID">ID for upload picture</param>
//        public MtrDeleteUploadPictureCmd(long uploadID)
//        {
//            UploadID = uploadID;
//        }
//        #endregion
//    }

//    /// <summary>
//    /// Mediatr Command-Handler for Delete upload picture command
//    /// </summary>
//    public class MtrDeleteUploadPictureCmdHandler : IRequestHandler<MtrDeleteUploadPictureCmd>
//    {
//        #region Private Members
//        private readonly iUnitOfWork _UnitOfWork;
//        private readonly ILogger<MtrDeleteUploadPictureCmdHandler> _Logger;
//        #endregion

//        #region C'tor
//        /// <summary>
//        /// C'tor
//        /// </summary>
//        /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
//        /// <param name="logger">Logger. Injected by DI</param>
//        public MtrDeleteUploadPictureCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrDeleteUploadPictureCmdHandler> logger)
//        {
//            //Übernehmen der injected Classes
//            _UnitOfWork = unitOfWork;
//            _Logger = logger;
//        }
//        #endregion

//        #region Mediatr-Handler
//        /// <summary>
//        /// Will be called by Mediatr
//        /// </summary>
//        /// <param name="request">The request data</param>
//        /// <param name="cancellationToken">The cancelation token</param>
//        /// <returns>Task</returns>
//        public async Task Handle(MtrDeleteUploadPictureCmd request, CancellationToken cancellationToken)
//        {
//            _Logger.LogInformation("Mediatr-Handler for Delete upload picture command was called with request: {@Request}", request);

//            _Logger.LogDebug("Get repository for UploadPicture");
//            iRepository<Domain.Entities.UploadPicture> Repository = _UnitOfWork.GetRepository<Domain.Entities.UploadPicture>();

//            _Logger.LogDebug("Get item from repository");
//            Domain.Entities.UploadPicture ItemDoDelete = await Repository.GetOneAsync(request.UploadID);

//            _Logger.LogDebug("Remove item from repository");
//            Repository.Delete(ItemDoDelete);

//            _Logger.LogDebug("Saving changes to data store");
//            await _UnitOfWork.SaveChangesAsync();
//        }
//        #endregion
//    }
//}
