using HotChocolate;
using HotChocolate.Types;
using MediatR;
using System.Threading.Tasks;
using User.API.Commands.UserCommands;

namespace User.API.GraphQL.Mutations.UserMutations
{
    [ExtendObjectType(typeof(Mutation))]
    public class MutationsUser
    {
        [GraphQLDescription("Add a new user")]
        //[Authorize(Policy = "User.Add")]
        public async Task<AddUserPayload> AddUserAsync(AddUserInput input, [Service] IMediator mediator)
        {
            var newUser = await mediator.Send(new MtrAddUserCmd(input));

            var result = new AddUserPayload
            {
                User = newUser
            };

            return result;
        }

        [GraphQLDescription("Update existing user")]
        //[Authorize(Policy = "User.Change")]
        public async Task<UpdateUserPayload> UpdateUserAsync(UpdateUserInput input, [Service] IMediator mediator)
        {
            var updatedUser = await mediator.Send(new MtrUpdateUserCmd(input));

            var result = new UpdateUserPayload
            {
                User = updatedUser
            };

            return result;
        }

        //[GraphQLDescription("Delete existing category")]
        //[Authorize(Policy = "Category.Delete")]
        //public async Task<DeleteCategoryPayload> DeleteCategoryAsync(DeleteCategoryInput input, [Service] IMediator mediator)
        //{
        //    var deletedCategory = await mediator.Send(new MtrDeleteCategoryCmd(input));

        //    var result = new DeleteCategoryPayload
        //    {
        //        Category = deletedCategory
        //    };

        //    return result;
        //}
    }
}
