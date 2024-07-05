//using Catalog.API.GraphQL.Mutations.MediaItem;
//using DomainHelper.Interfaces;
//using MediatR;

//namespace Catalog.API.Mediator.Commands.MediaItem;

///// <summary>
///// Mediatr Command for update media group entry
///// </summary>
//public class MtrUpdateMediaItemCmd : IRequest<Domain.Aggregates.MediaItem>
//{
//    #region Properties
//    /// <summary>
//    /// The input data from GraphQL Mutation
//    /// </summary>
//    public UpdateMediaItemInput InputData { get; private set; }
//    #endregion

//    #region C'tor
//    /// <summary>
//    /// C'tor
//    /// </summary>
//    /// <param name="inputData">The input data from GraphQL Mutation</param>
//    public MtrUpdateMediaItemCmd(UpdateMediaItemInput inputData)
//    {
//        InputData = inputData;
//    }
//    #endregion
//}

///// <summary>
///// Mediatr Command-Handler for update media item entry command
///// </summary>
//public class MtrUpdateMediaItemCmdHandler : IRequestHandler<MtrUpdateMediaItemCmd, Domain.Aggregates.MediaItem>
//{
//    #region Private Members
//    private readonly iUnitOfWork _UnitOfWork;
//    private readonly ILogger<MtrUpdateMediaItemCmdHandler> _Logger;
//    #endregion

//    #region C'tor
//    /// <summary>
//    /// C'tor
//    /// </summary>
//    /// <param name="unitOfWork">UnitOfWork will be injected by DI-Container</param>
//    /// <param name="logger">Logger. Injected by DI</param>
//    public MtrUpdateMediaItemCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrUpdateMediaItemCmdHandler> logger)
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
//    public async Task<Domain.Aggregates.MediaItem> Handle(MtrUpdateMediaItemCmd request, CancellationToken cancellationToken)
//    {
//        _Logger.LogInformation("Mediatr-Handler for update media item entry command was called for {@Input}", request.InputData);

//        _Logger.LogDebug("Get repository for media items");
//        var repository = _UnitOfWork.GetRepository<Domain.Aggregates.MediaItem>();

//        _Logger.LogDebug($"Get media item for id={request.InputData.Id}");
//        var mediaItem = await repository.GetOneAsync(request.InputData.Id);

//        _Logger.LogDebug("Updating media item domain model");
//        mediaItem.Update(request.InputData.NameGerman, request.InputData.NameEnglish,
//            request.InputData.DescriptionGerman, request.InputData.DescriptionEnglish, request.InputData.OnlyFamily);

//        _Logger.LogDebug("Updating assigned category values");
//        await mediaItem.UpdateCategoryValues(request.InputData.CategoryValues);

//        _Logger.LogDebug("Saving changes to data store");
//        await _UnitOfWork.SaveChangesAsync();

//        return mediaItem;
//    }
//    #endregion
//}
