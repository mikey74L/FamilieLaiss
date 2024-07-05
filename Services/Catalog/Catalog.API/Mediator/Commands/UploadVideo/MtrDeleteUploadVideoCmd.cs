//using DomainHelper.Interfaces;
//using MediatR;

//namespace Catalog.API.Mediator.Commands.UploadVideo
//{
//    /// <summary>
//    /// Mediatr Command for Delete upload Video
//    /// </summary>
//    public class MtrDeleteUploadVideoCmd : IRequest
//    {
//        #region Public Properties
//        /// <summary>
//        /// ID for upload Video
//        /// </summary>
//        public long UploadID { get; private set; }
//        #endregion

//        #region C'tor
//        /// <summary>
//        /// C'tor
//        /// </summary>
//        /// <param name="uploadVideoID">ID for upload Video</param>
//        public MtrDeleteUploadVideoCmd(long uploadID)
//        {
//            UploadID = uploadID;
//        }
//        #endregion
//    }

//    /// <summary>
//    /// Mediatr Command-Handler for Delete upload Video command
//    /// </summary>
//    public class MtrDeleteUploadVideoCmdHandler : IRequestHandler<MtrDeleteUploadVideoCmd>
//    {
//        #region Private Members
//        private readonly iUnitOfWork _UnitOfWork;
//        private readonly ILogger<MtrDeleteUploadVideoCmdHandler> _Logger;
//        #endregion

//        #region C'tor
//        /// <summary>
//        /// C'tor
//        /// </summary>
//        /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
//        /// <param name="logger">Logger. Injected by DI</param>
//        public MtrDeleteUploadVideoCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrDeleteUploadVideoCmdHandler> logger)
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
//        public async Task Handle(MtrDeleteUploadVideoCmd request, CancellationToken cancellationToken)
//        {
//            _Logger.LogInformation("Mediatr-Handler for Delete upload Video command was called with request: {@Request}", request);

//            _Logger.LogDebug("Get repository for UploadVideo");
//            iRepository<Domain.Entities.UploadVideo> Repository = _UnitOfWork.GetRepository<Domain.Entities.UploadVideo>();

//            _Logger.LogDebug("Get item from repository");
//            Domain.Entities.UploadVideo ItemDoDelete = await Repository.GetOneAsync(request.UploadID);

//            _Logger.LogDebug("Remove item from repository");
//            Repository.Delete(ItemDoDelete);

//            _Logger.LogDebug("Saving changes to data store");
//            await _UnitOfWork.SaveChangesAsync();
//        }
//        #endregion
//    }
//}
