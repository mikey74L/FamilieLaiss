namespace VideoConvert.API.Commands;

///// <summary>
///// Mediatr Command for Change the video info
///// </summary>
//public class MtrChangeVideoInfoCmd : IRequest
//{
//    #region Public Properties

//    /// <summary>
//    /// Id for the upload video
//    /// </summary>
//    public long ID { get; init; }

//    /// <summary>
//    /// Type of video
//    /// </summary>
//    public EnumVideoType VideoType { get; init; }

//    /// <summary>
//    /// Height of the original video
//    /// </summary>
//    public int Height { get; init; }

//    /// <summary>
//    /// Width of the original video
//    /// </summary>
//    public int Width { get; init; }

//    /// <summary>
//    /// The hour part for the duration of this video
//    /// </summary>
//    public int Duration_Hour { get; init; }

//    /// <summary>
//    /// The minute part for the duration of this video
//    /// </summary>
//    public int Duration_Minute { get; init; }

//    /// <summary>
//    /// The second part for the duration of this video
//    /// </summary>
//    public int Duration_Second { get; init; }

//    #endregion

//    #region C'tor

//    /// <summary>
//    /// C'tor
//    /// </summary>
//    /// <param name="id">The ID for the upload video</param>
//    /// <param name="videoType">Type of video</param>
//    /// <param name="height">Height of the original video</param>
//    /// <param name="width">Width of the original video</param>
//    /// <param name="durationHour">The hour part for the duration of this video</param>
//    /// <param name="durationMinute">The minute part for the duration of this video</param>
//    /// <param name="durationSecond">The second part for the duration of this video</param>
//    public MtrChangeVideoInfoCmd(long id, EnumVideoType videoType, int height, int width, int durationHour,
//        int durationMinute, int durationSecond)
//    {
//        //Übernehmen der Parameter in die Properties
//        ID = id;
//        VideoType = videoType;
//        Height = height;
//        Width = width;
//        Duration_Hour = durationHour;
//        Duration_Minute = durationMinute;
//        Duration_Second = durationSecond;
//    }

//    #endregion
//}

///// <summary>
///// Mediatr Command-Handler for Change video info command
///// </summary>
//public class MtrChangeVideoInfoCmdHandler : AsyncRequestHandler<MtrChangeVideoInfoCmd>
//{
//    #region Private Members

//    private readonly iUnitOfWork _UnitOfWork;
//    private readonly ILogger<MtrChangeVideoInfoCmdHandler> _Logger;

//    #endregion

//    #region C'tor

//    /// <summary>
//    /// C'tor
//    /// </summary>
//    /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
//    /// <param name="logger">Logger. Injected by DI</param>
//    public MtrChangeVideoInfoCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrChangeVideoInfoCmdHandler> logger)
//    {
//        //Übernehmen der injected classes
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
//    protected override async Task Handle(MtrChangeVideoInfoCmd request, CancellationToken cancellationToken)
//    {
//        //Logging ausgeben
//        _Logger.LogInformation("Mediatr-Handler for Change video info command was called with following parameters:");
//        _Logger.LogDebug($"ID             : {request.ID}");
//        _Logger.LogDebug($"Video-Type     : {request.VideoType}");
//        _Logger.LogDebug($"Height         : {request.Height}");
//        _Logger.LogDebug($"Width          : {request.Width}");
//        _Logger.LogDebug($"Duration-Hour  : {request.Duration_Hour}");
//        _Logger.LogDebug($"Duration-Minute: {request.Duration_Minute}");
//        _Logger.LogDebug($"Duration-Second: {request.Duration_Second}");

//        //Repository ermitteln
//        _Logger.LogDebug("Get repository for UploadVideo");
//        var Repository = _UnitOfWork.GetRepository<UploadVideo>();

//        //Ermitteln des zu ändernden Items aus dem Store
//        _Logger.LogDebug($"Get upload video domain model with id = {request.ID} from data store");
//        var ItemToChange = await Repository.GetOneAsync(request.ID);
//        if (ItemToChange == null)
//        {
//            throw new NoDataFoundException($"Could not find upload video with id = {request.ID}");
//        }

//        //Anpassen der Daten
//        _Logger.LogDebug("Updating domain model");
//        ItemToChange.UpdateVideoInfo(request.Height, request.Width, request.Duration_Hour, request.Duration_Minute,
//            request.Duration_Second);
//        Repository.Update(ItemToChange);

//        //Speichern der Daten
//        _Logger.LogDebug("Saving changes to data store");
//        _ = await _UnitOfWork.SaveChanges();
//    }

//    #endregion
//}