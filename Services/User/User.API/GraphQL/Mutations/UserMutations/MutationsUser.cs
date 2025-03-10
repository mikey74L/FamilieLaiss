using HotChocolate;
using HotChocolate.Types;
using MediatR;
using System.Threading.Tasks;
using User.API.Mediator.Commands.UserCommands;

namespace User.API.GraphQL.Mutations.UserMutations;

[ExtendObjectType(typeof(Mutation))]
public class MutationsUser
{
    [GraphQLDescription("Add a new user")]
    public async Task<AddUserPayload> AddUserAsync(AddUserInput input, [Service] IMediator mediator)
    {
        var newUser = await mediator.Send(new MtrAddUserCmd() { InputData = input });

        var result = new AddUserPayload
        {
            UserAccount = newUser
        };

        return result;
    }

    [GraphQLDescription("Update existing user")]
    public async Task<UpdateUserPayload> UpdateUserAsync(UpdateUserInput input, [Service] IMediator mediator)
    {
        var updatedUser = await mediator.Send(new MtrUpdateUserCmd() { InputData = input });

        var result = new UpdateUserPayload
        {
            UserAccount = updatedUser
        };

        return result;
    }
}
