namespace FLBackEnd.API.GraphQL.Mutations.UserSetting;

[GraphQLDescription("The result for a updated user setting")]
public class UpdateUserSettingPayload
{
    [GraphQLDescription("The updated user setting")]
    public Domain.Entities.UserSetting UserSetting { get; set; } = default!;
}
