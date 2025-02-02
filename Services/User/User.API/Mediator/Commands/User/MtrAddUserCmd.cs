using DomainHelper.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using User.API.GraphQL.Mutations.UserMutations;

namespace User.API.Mediator.Commands.UserCommands;

/// <summary>
/// Mediatr Command for add new user entry
/// </summary>
public class MtrAddUserCmd : IRequest<Domain.Aggregates.UserAccount>
{
    #region Properties
    /// <summary>
    /// The input data from GraphQL Mutation
    /// </summary>
    public required AddUserInput InputData { get; init; }
    #endregion
}

/// <summary>
/// Mediatr Command-Handler for Make new category entry command
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrAddUserCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrAddUserCmdHandler> logger) : IRequestHandler<MtrAddUserCmd, Domain.Aggregates.UserAccount>
{
    #region Mediatr-Handler
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancelation token</param>
    /// <returns>Task</returns>
    public async Task<Domain.Aggregates.UserAccount> Handle(MtrAddUserCmd request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for make new category entry command was called for {Input}", request.InputData);

        logger.LogDebug("Get repository for user");
        var Repository = unitOfWork.GetRepository<Domain.Aggregates.UserAccount>();

        logger.LogDebug("Adding new user domain model to repository");
        var NewUser = new Domain.Aggregates.UserAccount(request.InputData.Id, request.InputData.UserName, request.InputData.EMail,
            request.InputData.Gender, request.InputData.GivenName, request.InputData.FamilyName, request.InputData.Street,
            request.InputData.Hnr, request.InputData.Zip, request.InputData.City, request.InputData.CountryId);
        await Repository.AddAsync(NewUser);

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();

        return NewUser;
    }
    #endregion
}
