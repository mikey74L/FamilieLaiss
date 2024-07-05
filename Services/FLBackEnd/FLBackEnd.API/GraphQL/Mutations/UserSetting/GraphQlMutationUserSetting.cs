using FLBackEnd.API.Mediator.Commands.UserSetting;
using MediatR;

namespace FLBackEnd.API.GraphQL.Mutations.UserSetting;

[ExtendObjectType(typeof(Mutation))]
public class GraphQlMutationUserSetting
{
    [GraphQLDescription("Add a new user setting")]
    public async Task<AddUserSettingPayload> AddUserSettingAsync(AddUserSettingInput input, [Service] IMediator mediator)
    {
        var newCategory = await mediator.Send(new MtrAddUserSettingCmd() { InputData = input });

        var result = new AddUserSettingPayload
        {
            UserSetting = newCategory
        };

        return result;
    }

    [GraphQLDescription("Update existing user setting")]
    public async Task<UpdateUserSettingPayload> UpdateUserSettingAsync(UpdateUserSettingInput input,
        [Service] IMediator mediator)
    {
        var updatedCategory = await mediator.Send(new MtrUpdateUserSettingCmd() { InputData = input });

        var result = new UpdateUserSettingPayload
        {
            UserSetting = updatedCategory
        };

        return result;
    }
}
