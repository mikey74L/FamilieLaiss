using MediatR;
using Polly;
using Settings.API.Commands;

namespace Settings.API.GraphQL.Mutations.UserSettings
{
    [ExtendObjectType(typeof(Mutation))]
    public class MutationsUserSettings
    {
        [GraphQLDescription("Update existing category")]
        public async Task<UpdateUserSettingsPayload> UpdateUserSettingsAsync(UpdateUserSettingsInput input, [Service] IMediator mediator)
        {
            var updatedUserSettings = await mediator.Send(new MtrChangeUserSettingsCmd(input));

            var result = new UpdateUserSettingsPayload
            {
                UserSettings = updatedUserSettings
            };

            return result;
        }
    }
}
