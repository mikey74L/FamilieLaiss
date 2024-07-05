using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using User.API.GraphQL.Mutations.UserMutations;

namespace User.API.Commands.UserCommands
{
    /// <summary>
    /// Mediatr Command for updating existing user entry
    /// </summary>
    public class MtrUpdateUserCmd : IRequest<User.Domain.Aggregates.User>
    {
        #region Properties
        /// <summary>
        /// The input data from GraphQL Mutation
        /// </summary>
        public UpdateUserInput InputData { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="inputData">The input data from GraphQL Mutation</param>
        public MtrUpdateUserCmd(UpdateUserInput inputData)
        {
            InputData = inputData;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for updating existing user entry command
    /// </summary>
    public class MtrUpdateUserCmdHandler : IRequestHandler<MtrUpdateUserCmd, User.Domain.Aggregates.User>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly ILogger<MtrUpdateUserCmdHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrUpdateUserCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrUpdateUserCmdHandler> logger)
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
        public async Task<User.Domain.Aggregates.User> Handle(MtrUpdateUserCmd request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for change user entry command was called for {Input}", request.InputData);

            //Repository für User ermitteln
            _Logger.LogDebug("Get repository for user");
            var Repository = _UnitOfWork.GetRepository<User.Domain.Aggregates.User>();

            //User ermitteln
            _Logger.LogDebug("Get existing entity for {ID}", request.InputData.ID);
            var ModelToUpdate = await Repository.GetOneAsync(request.InputData.ID);
            if (ModelToUpdate == null)
            {
                _Logger.LogError("Could not find user with {ID}", request.InputData.ID);
                throw new DomainException(DomainExceptionType.NoDataFound, $"Could not find user with id = {request.InputData.ID}");
            }

            //User anpassen
            _Logger.LogDebug("Updating User-Data");
            ModelToUpdate.Update(request.InputData.GenderID, request.InputData.GivenName, request.InputData.FamilyName,
                request.InputData.Street, request.InputData.HNR, request.InputData.ZIP, request.InputData.City,
                request.InputData.CountryID);
            Repository.Update(ModelToUpdate);

            //Speichern der Daten
            _Logger.LogDebug("Saving changes to data store");
            await _UnitOfWork.SaveChangesAsync();

            //Funktionsergebnis
            return ModelToUpdate;
        }
        #endregion
    }
}
