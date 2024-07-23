using DomainHelper.Interfaces;
using MediatR;
using Settings.API.GraphQL.Mutations.UserSettings;
using Settings.Domain.Entities;

namespace Settings.API.Mediator.Commands;

/// <summary>
/// Mediatr Command for add user setting entry
/// </summary>
public class MtrAddUserSettingCmd : IRequest<UserSetting>
{
    /// <summary>
    /// The input data from GraphQL Mutation
    /// </summary>
    public required AddUserSettingInput InputData { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for add user setting entry command
/// </summary>
/// <remarks>
/// Primary constructor
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrAddUserSettingCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrAddUserSettingCmdHandler> logger)
    : IRequestHandler<MtrAddUserSettingCmd, Domain.Entities.UserSetting>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task<Domain.Entities.UserSetting> Handle(MtrAddUserSettingCmd request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for make new user setting entry command was called for {@InputData}",
            request.InputData);

        logger.LogDebug("Get repository for user setting");
        var repository = unitOfWork.GetRepository<Domain.Entities.UserSetting>();

        logger.LogDebug("Adding new user setting domain model to repository");
        var newUserSetting = new Domain.Entities.UserSetting(request.InputData.Id);
        await repository.AddAsync(newUserSetting);

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();

        return newUserSetting;
    }
}