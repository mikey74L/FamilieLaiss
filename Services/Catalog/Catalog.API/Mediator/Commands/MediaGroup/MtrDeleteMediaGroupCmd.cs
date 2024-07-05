//using Catalog.API.GraphQL.Mutations.MediaGroup;
//using DomainHelper.Exceptions;
//using DomainHelper.Interfaces;
//using MediatR;

//namespace Catalog.API.Mediator.Commands.MediaGroup
//{
//    /// <summary>
//    /// Mediatr Command for delete media group
//    /// </summary>
//    public class MtrDeleteMediaGroupCmd : IRequest<Domain.Aggregates.MediaGroup>
//    {
//        #region Properties
//        /// <summary>
//        /// Input data from GraphQL
//        /// </summary>
//        public DeleteMediaGroupInput Input { get; private set; }
//        #endregion

//        #region C'tor
//        /// <summary>
//        /// C'tor
//        /// </summary>
//        /// <param name="input">Input data from GraphQL</param>
//        public MtrDeleteMediaGroupCmd(DeleteMediaGroupInput input)
//        {
//            Input = input;
//        }
//        #endregion
//    }

//    /// <summary>
//    /// Mediatr Command-Handler for delete media group
//    /// </summary>
//    public class MtrDeleteMediaGroupCmdHandler : IRequestHandler<MtrDeleteMediaGroupCmd, Domain.Aggregates.MediaGroup>
//    {
//        #region Private Members
//        private readonly iUnitOfWork _UnitOfWork;
//        private readonly ILogger<MtrDeleteMediaGroupCmdHandler> _Logger;
//        #endregion

//        #region C'tor
//        /// <summary>
//        /// C'tor
//        /// </summary>
//        /// <param name="unitOfWork">Unit of work. Will be injected by DI.</param>
//        /// <param name="logger">Logger. Injected by DI</param>
//        public MtrDeleteMediaGroupCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrDeleteMediaGroupCmdHandler> logger)
//        {
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
//        public async Task<Domain.Aggregates.MediaGroup> Handle(MtrDeleteMediaGroupCmd request, CancellationToken cancellationToken)
//        {
//            //Logging ausgeben
//            _Logger.LogInformation("Mediatr-Handler for delete media group command was called for {@Input}", request.Input);

//            //Repository für Kategorie ermitteln
//            _Logger.LogDebug("Get repository for media group");
//            var Repository = _UnitOfWork.GetRepository<Domain.Aggregates.MediaGroup>();

//            //Ermitteln des bisherigen Models
//            _Logger.LogDebug("Find model to delete in store");
//            var ModelToDelete = await Repository.GetOneAsync(request.Input.id);
//            if (ModelToDelete == null)
//            {
//                _Logger.LogError("Could not find media group with id {ID}", request.Input.id);
//                throw new NoDataFoundException($"Could not find media group with id = {request.Input.id}");
//            }

//            //Eine neue Kategorie erstellen
//            _Logger.LogDebug("Delete media group domain model from store");
//            Repository.Delete(ModelToDelete);

//            //Speichern der Daten
//            _Logger.LogDebug("Saving changes to data store");
//            await _UnitOfWork.SaveChangesAsync();

//            return ModelToDelete;
//        }
//        #endregion
//    }
//}
