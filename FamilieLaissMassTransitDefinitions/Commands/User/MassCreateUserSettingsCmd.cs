using FamilieLaissMassTransitDefinitions.Contracts.Commands.User;

namespace FamilieLaissMassTransitDefinitions.Commands.User;

public class MassCreateUserSettingsCmd : IMassCreateUserSettingsCmd
{
    public required string Id { get; init; }
}
