namespace Settings.API.MassTransit.Consumers;

///// <summary>
///// MassTransit consumer for "UserAccountCreated"-Event
///// </summary>
//public class UserAccountCreatedConsumer : IConsumer<iUserAccountCreatedEvent>
//{
//    #region Private Members
//    private readonly iUnitOfWork _UnitOfWork;
//    private readonly ILogger<UserAccountCreatedConsumer> _Logger;
//    #endregion

//    #region C'tor
//    /// <summary>
//    /// C'tor
//    /// </summary>
//    /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
//    /// <param name="logger">Logger. Injected by DI</param>
//    public UserAccountCreatedConsumer(iUnitOfWork unitOfWork, ILogger<UserAccountCreatedConsumer> logger)
//    {
//        //Übernehmen der Injected Objects
//        _UnitOfWork = unitOfWork;
//        _Logger = logger;
//    }
//    #endregion

//    #region Interface IConsumer
//    /// <summary>
//    /// Would be called from Masstransit
//    /// </summary>
//    /// <param name="context">The context for this event</param>
//    /// <returns>Task</returns>
//    public async Task Consume(ConsumeContext<iUserAccountCreatedEvent> context)
//    {
//        //Ausgeben von Logging
//        _Logger.LogInformation("Consumer for User Account Created Event was called");

//        //Ermitteln des Repositories für den User-Account
//        _Logger.LogDebug("Get repository for user settings");
//        var RepositorySettings = _UnitOfWork.GetRepository<UserSettings>();

//        //Hinzufügen der User-Settings
//        _Logger.LogDebug("Add new user settings");
//        var NewUserSettings = new UserSettings(context.Message.UserName);
//        await RepositorySettings.AddAsync(NewUserSettings);

//        //Speichern der Änderungen
//        _Logger.LogDebug("Saving changes to store");
//        await _UnitOfWork.SaveChangesAsync();
//    }
//    #endregion
//}
