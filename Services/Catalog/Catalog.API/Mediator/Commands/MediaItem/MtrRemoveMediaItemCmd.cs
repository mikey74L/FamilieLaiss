//using Catalog.API.GraphQL.Mutations.MediaItem;
//using DomainHelper.Interfaces;
//using MediatR;

//namespace Catalog.API.Mediator.Commands.MediaItem;

///// <summary>
///// Mediatr Command for remove media item
///// </summary>
//public class MtrRemoveMediaItemCmd : IRequest<Domain.Aggregates.MediaItem>
//{
//    #region Properties
//    /// <summary>
//    /// The input data from GraphQL Mutation
//    /// </summary>
//    public RemoveMediaItemInput InputData { get; private set; }
//    #endregion

//    #region C'tor
//    /// <summary>
//    /// C'tor
//    /// </summary>
//    /// <param name="inputData">The input data from GraphQL Mutation</param>
//    public MtrRemoveMediaItemCmd(RemoveMediaItemInput inputData)
//    {
//        InputData = inputData;
//    }
//    #endregion
//}

///// <summary>
///// Mediatr Command-Handler for remove media item command
///// </summary>
//public class MtrRemoveMediaItemCmdHandler : IRequestHandler<MtrRemoveMediaItemCmd, Domain.Aggregates.MediaItem>
//{
//    #region Private Members
//    private readonly iUnitOfWork _UnitOfWork;
//    private readonly ILogger<MtrRemoveMediaItemCmdHandler> _Logger;
//    #endregion

//    #region C'tor
//    /// <summary>
//    /// C'tor
//    /// </summary>
//    /// <param name="unitOfWork">UnitOfWork will be injected by DI-Container</param>
//    /// <param name="logger">Logger. Injected by DI</param>
//    public MtrRemoveMediaItemCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrRemoveMediaItemCmdHandler> logger)
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
//    public async Task<Domain.Aggregates.MediaItem> Handle(MtrRemoveMediaItemCmd request, CancellationToken cancellationToken)
//    {
//        _Logger.LogInformation("Mediatr-Handler for remove media item command was called for {@Input}", request.InputData);

//        _Logger.LogDebug("Get repository for media groups");
//        var repository = _UnitOfWork.GetRepository<Domain.Aggregates.MediaGroup>();

//        //Ermitteln der Media-Group
//        _Logger.LogDebug($"Get media group for id={request.InputData.MediaGroupId}");
//        var mediaGroup = await repository.GetOneAsync(request.InputData.MediaGroupId);

//        //Ein neues Media-Item erstellen
//        _Logger.LogDebug("Remove media item domain model from media group");
//        var mediaItem = await mediaGroup.RemoveMediaItem(request.InputData.MediaItemId, request.InputData.DeleteUploadItem);

//        //Speichern der Daten
//        _Logger.LogDebug("Saving changes to data store");
//        await _UnitOfWork.SaveChangesAsync();

//        //Funktionsergebnis
//        return mediaItem;
//    }
//    #endregion
//}
