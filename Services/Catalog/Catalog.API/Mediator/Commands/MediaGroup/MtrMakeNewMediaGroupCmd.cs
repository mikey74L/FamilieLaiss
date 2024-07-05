//using Catalog.API.GraphQL.Mutations.MediaGroup;
//using DomainHelper.Interfaces;
//using MediatR;

//namespace Catalog.API.Mediator.Commands.MediaGroup
//{
//    /// <summary>
//    /// Mediatr Command for make new media group entry
//    /// </summary>
//    public class MtrMakeNewMediaGroupCmd : IRequest<Domain.Aggregates.MediaGroup>
//    {
//        #region Properties
//        /// <summary>
//        /// The input data from GraphQL Mutation
//        /// </summary>
//        public AddMediaGroupInput InputData { get; private set; }
//        #endregion

//        #region C'tor
//        /// <summary>
//        /// C'tor
//        /// </summary>
//        /// <param name="inputData">The input data from GraphQL Mutation</param>
//        public MtrMakeNewMediaGroupCmd(AddMediaGroupInput inputData)
//        {
//            InputData = inputData;
//        }
//        #endregion
//    }

//    /// <summary>
//    /// Mediatr Command-Handler for Make new media group entry command
//    /// </summary>
//    public class MtrMakeNewMediaGroupCmdHandler : IRequestHandler<MtrMakeNewMediaGroupCmd, Domain.Aggregates.MediaGroup>
//    {
//        #region Private Members
//        private readonly iUnitOfWork _UnitOfWork;
//        private readonly ILogger<MtrMakeNewMediaGroupCmdHandler> _Logger;
//        #endregion

//        #region C'tor
//        /// <summary>
//        /// C'tor
//        /// </summary>
//        /// <param name="unitOfWork">UnitOfWork will be injected by DI-Container</param>
//        /// <param name="mapper">The automapper instence. Will be injected by DI-Container</param>
//        /// <param name="logger">Logger. Injected by DI</param>
//        public MtrMakeNewMediaGroupCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrMakeNewMediaGroupCmdHandler> logger)
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
//        public async Task<Domain.Aggregates.MediaGroup> Handle(MtrMakeNewMediaGroupCmd request, CancellationToken cancellationToken)
//        {
//            //Logging ausgeben
//            _Logger.LogInformation("Mediatr-Handler for make new media group entry command was called for {@Input}", request.InputData);

//            //Repository für Kategorie ermitteln
//            _Logger.LogDebug("Get repository for media group");
//            var Repository = _UnitOfWork.GetRepository<Domain.Aggregates.MediaGroup>();

//            //Eine neue Kategorie erstellen
//            _Logger.LogDebug("Adding new media group domain model to repository");
//            var NewMediaGroup = new Domain.Aggregates.MediaGroup(request.InputData.NameGerman,
//                request.InputData.NameEnglish, request.InputData.DescriptionGerman, request.InputData.DescriptionEnglish,
//                request.InputData.EventDate);
//            await Repository.AddAsync(NewMediaGroup);

//            //Speichern der Daten
//            _Logger.LogDebug("Saving changes to data store");
//            await _UnitOfWork.SaveChangesAsync();

//            //Funktionsergebnis
//            return NewMediaGroup;
//        }
//        #endregion
//    }
//}
