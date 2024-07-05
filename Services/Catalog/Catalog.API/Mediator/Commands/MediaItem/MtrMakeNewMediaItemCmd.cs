//using Catalog.API.GraphQL.Mutations.MediaItem;
//using DomainHelper.Interfaces;
//using MediatR;

//namespace Catalog.API.Mediator.Commands.MediaItem;

///// <summary>
///// Mediatr Command for make new media group entry
///// </summary>
//public class MtrMakeNewMediaItemCmd : IRequest<Domain.Aggregates.MediaItem>
//{
//    #region Properties
//    /// <summary>
//    /// The input data from GraphQL Mutation
//    /// </summary>
//    public AddMediaItemInput InputData { get; private set; }
//    #endregion

//    #region C'tor
//    /// <summary>
//    /// C'tor
//    /// </summary>
//    /// <param name="inputData">The input data from GraphQL Mutation</param>
//    public MtrMakeNewMediaItemCmd(AddMediaItemInput inputData)
//    {
//        InputData = inputData;
//    }
//    #endregion
//}

///// <summary>
///// Mediatr Command-Handler for Make new media item entry command
///// </summary>
//public class MtrMakeNewMediaItemCmdHandler : IRequestHandler<MtrMakeNewMediaItemCmd, Domain.Aggregates.MediaItem>
//{
//    #region Private Members
//    private readonly iUnitOfWork _UnitOfWork;
//    private readonly ILogger<MtrMakeNewMediaItemCmdHandler> _Logger;
//    #endregion

//    #region C'tor
//    /// <summary>
//    /// C'tor
//    /// </summary>
//    /// <param name="unitOfWork">UnitOfWork will be injected by DI-Container</param>
//    /// <param name="logger">Logger. Injected by DI</param>
//    public MtrMakeNewMediaItemCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrMakeNewMediaItemCmdHandler> logger)
//    {
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
//    public async Task<Domain.Aggregates.MediaItem> Handle(MtrMakeNewMediaItemCmd request, CancellationToken cancellationToken)
//    {
//        _Logger.LogInformation("Mediatr-Handler for make new media item entry command was called for {@Input}", request.InputData);

//        _Logger.LogDebug("Get repository for media groups");
//        var repository = _UnitOfWork.GetRepository<Domain.Aggregates.MediaGroup>();

//        _Logger.LogDebug($"Get media group for id={request.InputData.MediaGroupID}");
//        var mediaGroup = await repository.GetOneAsync(request.InputData.MediaGroupID);

//        _Logger.LogDebug("Adding new media item domain model to repository");
//        var mediaItem = mediaGroup.AddMediaItem(request.InputData.MediaType, request.InputData.NameGerman, request.InputData.NameEnglish,
//            request.InputData.DescriptionGerman, request.InputData.DescriptionEnglish, request.InputData.OnlyFamily,
//            request.InputData.UploadID);

//        _Logger.LogDebug("Assign category values to media item");
//        foreach (var item in request.InputData.CategoryValues)
//        {
//            mediaItem.AddCategoryValue(item);
//        }

//        _Logger.LogDebug("Saving changes to data store");
//        await _UnitOfWork.SaveChangesAsync();

//        return mediaItem;
//    }
//    #endregion
//}
