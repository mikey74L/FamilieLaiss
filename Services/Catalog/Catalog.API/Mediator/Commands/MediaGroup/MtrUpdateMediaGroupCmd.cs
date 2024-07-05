//using Catalog.API.GraphQL.Mutations.MediaGroup;
//using DomainHelper.Exceptions;
//using DomainHelper.Interfaces;
//using MediatR;

//namespace Catalog.API.Mediator.Commands.MediaGroup
//{
//    /// <summary>
//    /// Mediatr Command for update media group 
//    /// </summary>
//    public class MtrUpdateMediaGroupCmd : IRequest<Domain.Aggregates.MediaGroup>
//    {
//        #region Properties
//        /// <summary>
//        /// GraphQL input data
//        /// </summary>
//        public UpdateMediaGroupInput Input { get; private set; }
//        #endregion

//        #region C'tor
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="input">GraphQL input data</param>
//        public MtrUpdateMediaGroupCmd(UpdateMediaGroupInput input)
//        {
//            Input = input;
//        }
//        #endregion
//    }

//    /// <summary>
//    /// Mediatr Command-Handler for update media group
//    /// </summary>
//    public class MtrUpdateMediaGroupCmdHandler : IRequestHandler<MtrUpdateMediaGroupCmd, Domain.Aggregates.MediaGroup>
//    {
//        #region Private Members
//        private readonly iUnitOfWork _UnitOfWork;
//        private readonly ILogger<MtrUpdateMediaGroupCmdHandler> _Logger;
//        #endregion

//        #region C'tor
//        /// <summary>
//        /// C'tor
//        /// </summary>
//        /// <param name="unitOfWork">Unit of work will be injected by DI-Container</param>
//        /// <param name="logger">Logger. Injected by DI.</param>
//        public MtrUpdateMediaGroupCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrUpdateMediaGroupCmdHandler> logger)
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
//        public async Task<Domain.Aggregates.MediaGroup> Handle(MtrUpdateMediaGroupCmd request, CancellationToken cancellationToken)
//        {
//            //Logging ausgeben
//            _Logger.LogInformation("Mediatr-Handler for update media group command was called for {@Input}", request.Input);

//            //Repository für Media-Group ermitteln
//            _Logger.LogDebug("Get repository for media group");
//            var Repository = _UnitOfWork.GetRepository<Domain.Aggregates.MediaGroup>();

//            //Ermitteln des bisherigen Models
//            _Logger.LogDebug("Find model to update in store");
//            var ModelToUpdate = await Repository.GetOneAsync(request.Input.id);
//            if (ModelToUpdate == null)
//            {
//                _Logger.LogError("Could not find media group with {ID}", request.Input.id);
//                throw new NoDataFoundException($"Could not find media group with id = {request.Input.id}");
//            }

//            //Media-Group aktualisieren
//            _Logger.LogDebug("Update media group data");
//            ModelToUpdate.Update(request.Input.NameGerman, request.Input.NameEnglish, request.Input.DescriptionGerman,
//                request.Input.DescriptionEnglish, request.Input.EventDate);
//            Repository.Update(ModelToUpdate);

//            //Speichern der Daten
//            _Logger.LogDebug("Saving changes to data store");
//            await _UnitOfWork.SaveChangesAsync();

//            return ModelToUpdate;
//        }
//        #endregion
//    }
//}
