using DomainHelper.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using User.API.GraphQL.Mutations.UserMutations;

namespace User.API.Commands.UserCommands
{
    /// <summary>
    /// Mediatr Command for add new user entry
    /// </summary>
    public class MtrAddUserCmd : IRequest<User.Domain.Aggregates.User>
    {
        #region Properties
        /// <summary>
        /// The input data from GraphQL Mutation
        /// </summary>
        public AddUserInput InputData { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="inputData">The input data from GraphQL Mutation</param>
        public MtrAddUserCmd(AddUserInput inputData)
        {
            InputData = inputData;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for Make new category entry command
    /// </summary>
    public class MtrAddUserCmdHandler : IRequestHandler<MtrAddUserCmd, User.Domain.Aggregates.User>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly ILogger<MtrAddUserCmdHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrAddUserCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrAddUserCmdHandler> logger)
        {
            _UnitOfWork = unitOfWork;
            _Logger = logger;
        }
        #endregion

        #region Mediatr-Handler
        /// <summary>
        /// Will be called by Mediatr
        /// </summary>
        /// <param name="request">The request data</param>
        /// <param name="cancellationToken">The cancelation token</param>
        /// <returns>Task</returns>
        public async Task<User.Domain.Aggregates.User> Handle(MtrAddUserCmd request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for make new category entry command was called for {Input}", request.InputData);

            //Repository für User ermitteln
            _Logger.LogDebug("Get repository for user");
            var Repository = _UnitOfWork.GetRepository<User.Domain.Aggregates.User>();

            //Einen neuen User erstellen
            _Logger.LogDebug("Adding new user domain model to repository");
            var NewUser = new Domain.Aggregates.User(request.InputData.ID, request.InputData.EMail, request.InputData.UserName);
            await Repository.AddAsync(NewUser);

            //Speichern der Daten
            _Logger.LogDebug("Saving changes to data store");
            await _UnitOfWork.SaveChangesAsync();

            //Funktionsergebnis
            return NewUser;
        }
        #endregion
    }
}
