namespace VideoConvert.API.Consumers;

///// <summary>
///// MassTransit consumer for "VideoUploaded"-Event
///// </summary>
//public class VideoInfoChangedConsumer : IConsumer<iVideoInfoChangedEvent>
//{
//    #region Private Members

//    private readonly IMediator _Mediator;
//    private readonly ILogger<VideoInfoChangedConsumer> _Logger;

//    #endregion

//    #region C'tor

//    /// <summary>
//    /// C'tor
//    /// </summary>
//    /// <param name="mediator">Mediatr class. Will be injected by IOC</param>
//    /// <param name="logger">Logger. Injected by DI</param>
//    public VideoInfoChangedConsumer(IMediator mediator, ILogger<VideoInfoChangedConsumer> logger)
//    {
//        //Übernehmen der Injected Objects
//        _Mediator = mediator;
//        _Logger = logger;
//    }

//    #endregion

//    #region Interface IConsumer

//    /// <summary>
//    /// Would be called from Masstransit
//    /// </summary>
//    /// <param name="context">The context for this event</param>
//    /// <returns>Task</returns>
//    public async Task Consume(ConsumeContext<iVideoInfoChangedEvent> context)
//    {
//        //Ausgeben von Logging
//        _Logger.LogInformation("Consumer for Video-Info-Changed Event was called with following parameters:");
//        _Logger.LogDebug($"ID             : {context.Message.ID}");
//        _Logger.LogDebug($"Video-Type     : {context.Message.VideoType}");
//        _Logger.LogDebug($"Height         : {context.Message.Height}");
//        _Logger.LogDebug($"Width          : {context.Message.Width}");
//        _Logger.LogDebug($"Duration-Hour  : {context.Message.Duration_Hour}");
//        _Logger.LogDebug($"Duration-Minute: {context.Message.Duration_Minute}");
//        _Logger.LogDebug($"Duration-Second: {context.Message.Duration_Second}");

//        //Command zum Ändern der Picture info ausführen
//        _Logger.LogDebug("Calling Mediatr command");
//        await _Mediator.Send(new MtrChangeVideoInfoCmd(context.Message.ID, context.Message.VideoType,
//            context.Message.Height, context.Message.Width,
//            context.Message.Duration_Hour, context.Message.Duration_Minute, context.Message.Duration_Second));
//    }

//    #endregion
//}