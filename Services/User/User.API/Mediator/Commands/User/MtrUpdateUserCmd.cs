using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using User.API.GraphQL.Mutations.UserMutations;

namespace User.API.Mediator.Commands.UserCommands;

/// <summary>
/// Mediatr Command for updating existing user entry
/// </summary>
public class MtrUpdateUserCmd : IRequest<Domain.Aggregates.UserAccount>
{
    #region Properties
    /// <summary>
    /// The input data from GraphQL Mutation
    /// </summary>
    public required UpdateUserInput InputData { get; init; }
    #endregion
}

/// <summary>
/// Mediatr Command-Handler for updating existing user entry command
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrUpdateUserCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrUpdateUserCmdHandler> logger) : IRequestHandler<MtrUpdateUserCmd, Domain.Aggregates.UserAccount>
{
    #region Mediatr-Handler
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancelation token</param>
    /// <returns>Task</returns>
    public async Task<Domain.Aggregates.UserAccount> Handle(MtrUpdateUserCmd request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for change user entry command was called for {Input}", request.InputData);

        logger.LogDebug("Get repository for user");
        var Repository = unitOfWork.GetRepository<Domain.Aggregates.UserAccount>();

        logger.LogDebug("Get existing entity for {ID}", request.InputData.Id);
        var ModelToUpdate = await Repository.GetOneAsync(request.InputData.Id);
        if (ModelToUpdate == null)
        {
            logger.LogError("Could not find user with {ID}", request.InputData.Id);
            throw new DomainException(DomainExceptionType.NoDataFound, $"Could not find user with id = {request.InputData.Id}");
        }

        logger.LogDebug("Updating User-Data");
        ModelToUpdate.Update(request.InputData.Gender, request.InputData.GivenName, request.InputData.FamilyName,
            request.InputData.Street, request.InputData.Hnr, request.InputData.Zip, request.InputData.City,
            request.InputData.CountryId);
        Repository.Update(ModelToUpdate);

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();

        return ModelToUpdate;
    }
    #endregion
}
