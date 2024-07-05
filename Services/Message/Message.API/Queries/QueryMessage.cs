using DomainHelper.Interfaces;
using FamilieLaissSharedObjects.Enums;
using Mapster;
using MediatR;

namespace Message.API.Queries
{
    // /// <summary>
    // /// Query for get messages
    // /// </summary>
    // public class QueryMessage : IRequest<IEnumerable<MessageDTOModel>>
    // {
    //     #region Properties
    //     /// <summary>
    //     /// Priority of message
    //     /// </summary>
    //     public enMessagePrio Prio { get; private set; }
    //
    //     /// <summary>
    //     /// Username for current user
    //     /// </summary>
    //     public string Username { get; private set; }
    //     #endregion
    //
    //     #region C'tor
    //     /// <summary>
    //     /// C'tor
    //     /// </summary>
    //     /// <param name="prio">Priority of message</param>
    //     /// <param name="userName">Username for current user</param>
    //     public QueryMessage(enMessagePrio prio, string userName)
    //     {
    //         Prio = prio;
    //         Username = userName;
    //     }
    //     #endregion
    // }
    //
    // /// <summary>
    // /// Mediatr-Query-Handler for get messages
    // /// </summary>
    // public class QueryHandlerMessage : IRequestHandler<QueryMessage, IEnumerable<MessageDTOModel>>
    // {
    //     #region Private Members
    //     private readonly iUnitOfWork _UnitOfWork;
    //     private readonly ILogger<QueryHandlerMessage> _Logger;
    //     #endregion
    //
    //     #region C'tor
    //     /// <summary>
    //     /// C'tor
    //     /// </summary>
    //     /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
    //     /// <param name="logger">Logger. Injected by DI</param>
    //     public QueryHandlerMessage(iUnitOfWork unitOfWork, ILogger<QueryHandlerMessage> logger)
    //     {
    //         //Übernehmen der injected Objects
    //         _UnitOfWork = unitOfWork;
    //         _Logger = logger;
    //     }
    //     #endregion
    //
    //     #region Query-Handler
    //     /// <summary>
    //     /// Will be called by Mediatr
    //     /// </summary>
    //     /// <param name="request">The request data</param>
    //     /// <param name="cancellationToken">The cancelation token</param>
    //     /// <returns>Task</returns>
    //     public async Task<IEnumerable<MessageDTOModel>> Handle(QueryMessage request, CancellationToken cancellationToken)
    //     {
    //         //Logging ausgeben
    //         _Logger.LogInformation("Mediatr-Query-Handler for messages was called with following parameters:");
    //         _Logger.LogDebug($"User-Name: {request.Username.Substring(1, 3)}");
    //         _Logger.LogDebug($"Prio     : {request.Prio}");
    //
    //         //Repository ermitteln
    //         _Logger.LogDebug("Create repository");
    //         var Repository = _UnitOfWork.GetReadOnlyRepository<Message.Domain.Aggregates.Message>();
    //
    //         //Ermitteln der Daten
    //         _Logger.LogDebug("Get data from repository");
    //         IEnumerable<Message.Domain.Aggregates.Message> Entities = await Repository.GetAll(x => x.Prio == request.Prio && x.MessageUsers.Any(y => y.UserName == request.Username && !y.Readed));
    //
    //         //Mit Automapper in DTOs umwandeln
    //         _Logger.LogDebug("Create DTO objects with automapper");
    //         var MappedEntities = Entities.Adapt<IEnumerable<MessageDTOModel>>();
    //
    //         //Result zurückliefern
    //         _Logger.LogDebug("Result");
    //         return MappedEntities;
    //     }
    //     #endregion
    // }
}
