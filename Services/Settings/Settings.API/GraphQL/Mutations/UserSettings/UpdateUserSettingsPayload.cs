namespace Settings.API.GraphQL.Mutations.UserSettings
{
    [GraphQLDescription("The result for updated user settings")]
    public class UpdateUserSettingsPayload
    {
        [GraphQLDescription("The updated user settings")]
        public Settings.Domain.Entities.UserSettings UserSettings { get; set; }
    }
}
