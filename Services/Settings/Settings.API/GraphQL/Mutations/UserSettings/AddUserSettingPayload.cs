namespace Settings.API.GraphQL.Mutations.UserSettings;

[GraphQLDescription("The result for a new added user setting")]
public class AddUserSettingPayload
{
    [GraphQLDescription("The new added user setting")]
    public Domain.Entities.UserSetting UserSetting { get; set; } = default!;
}
