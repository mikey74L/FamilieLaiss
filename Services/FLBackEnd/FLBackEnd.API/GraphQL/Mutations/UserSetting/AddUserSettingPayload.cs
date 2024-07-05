namespace FLBackEnd.API.GraphQL.Mutations.UserSetting;

[GraphQLDescription("The result for a new added user setting")]
public class AddUserSettingPayload
{
    [GraphQLDescription("The new added user setting")]
    public Domain.Entities.UserSetting UserSetting { get; set; } = default!;
}
