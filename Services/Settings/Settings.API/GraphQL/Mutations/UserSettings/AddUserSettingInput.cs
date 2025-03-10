namespace Settings.API.GraphQL.Mutations.UserSettings;

[GraphQLDescription("InputData type for adding user setting")]
public class AddUserSettingInput
{
    [GraphQLDescription("The id for the user")]
    public string Id { get; set; } = string.Empty;
}
