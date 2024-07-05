//using DomainHelper.Interfaces;
//using MediatR;

//namespace Catalog.API.Mediator.Commands.UploadVideo;

///// <summary>
///// Mediatr Command for create upload video entry
///// </summary>
//public class MtrCreateUploadVideoCmd : IRequest
//{
//    #region Public Properties
//    /// <summary>
//    /// The ID for the upload video
//    /// </summary>
//    public long ID { get; private set; }

//    /// <summary>
//    /// The original filename for the upload video
//    /// </summary>
//    public string Filename { get; private set; }
//    #endregion

//    #region C'tor
//    /// <summary>
//    /// C'tor
//    /// </summary>
//    /// <param name="id">The ID for the upload video</param>
//    /// <param name="filename">The original filename for the upload video</param>
//    public MtrCreateUploadVideoCmd(long id, string filename)
//    {
//        ID = id;
//        Filename = filename;
//    }
//    #endregion
//}

///// <summary>
///// Mediatr Command-Handler for create upload picture entry command
///// </summary>
//public class MtrCreateUploadVideoCmdHandler : IRequestHandler<MtrCreateUploadVideoCmd>
//{
//    #region Private Members
//    private readonly iUnitOfWork _UnitOfWork;
//    private readonly ILogger<MtrCreateUploadVideoCmdHandler> _Logger;
//    #endregion

//    #region C'tor
//    /// <summary>
//    /// C'tor
//    /// </summary>
//    /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
//    /// <param name="logger">Logger. Injected by DI</param>
//    public MtrCreateUploadVideoCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrCreateUploadVideoCmdHandler> logger)
//    {
//        //Übernehmen der injected Classes
//        _UnitOfWork = unitOfWork;
//        _Logger = logger;
//    }
//    #endregion

//    #region Mediatr-Handler
//    /// <summary>
//    /// Will be called by Mediatr
//    /// </summary>
//    /// <param name="request">The request data</param>
//    /// <param name="cancellationToken">The cancelation token</param>
//    /// <returns>Task</returns>
//    public async Task Handle(MtrCreateUploadVideoCmd request, CancellationToken cancellationToken)
//    {
//        _Logger.LogInformation("Mediatr-Handler for create upload picture command was called with request: {@Request}", request);

//        _Logger.LogDebug("Get repository for UploadVideo");
//        iRepository<Domain.Entities.UploadVideo> RepositoryPicture = _UnitOfWork.GetRepository<Domain.Entities.UploadVideo>();

//        _Logger.LogDebug("Adding new UploadVideo domain model to repository");
//        Domain.Entities.UploadVideo NewPicture = new(request.ID, request.Filename);
//        await RepositoryPicture.AddAsync(NewPicture);

//        _Logger.LogDebug("Saving changes to data store");
//        await _UnitOfWork.SaveChangesAsync();
//    }
//    #endregion
//}
