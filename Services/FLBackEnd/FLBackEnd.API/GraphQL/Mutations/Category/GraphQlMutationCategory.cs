using FLBackEnd.API.Mediator.Commands.Category;
using MediatR;

namespace FLBackEnd.API.GraphQL.Mutations.Category;

[ExtendObjectType(typeof(Mutation))]
public class GraphQlMutationCategory
{
    //[Authorize(Policy = "Category.Add")]
    [GraphQLDescription("Add a new category")]
    public async Task<AddCategoryPayload> AddCategoryAsync(AddCategoryInput input, [Service] IMediator mediator)
    {
        var newCategory = await mediator.Send(new MtrAddCategoryCommand() { InputData = input });

        var result = new AddCategoryPayload
        {
            Category = newCategory
        };

        return result;
    }

    //[Authorize("Category.Change")]
    [GraphQLDescription("Update existing category")]
    public async Task<UpdateCategoryPayload> UpdateCategoryAsync(UpdateCategoryInput input,
        [Service] IMediator mediator)
    {
        var updatedCategory = await mediator.Send(new MtrUpdateCategoryCmd() { InputData = input });

        var result = new UpdateCategoryPayload
        {
            Category = updatedCategory
        };

        return result;
    }

    //[Authorize(Policy = "Category.Delete")]
    [GraphQLDescription("Delete existing category")]
    public async Task<DeleteCategoryPayload> DeleteCategoryAsync(DeleteCategoryInput input,
        [Service] IMediator mediator)
    {
        var deletedCategory = await mediator.Send(new MtrDeleteCategoryCmd() { InputData = input });

        var result = new DeleteCategoryPayload
        {
            Category = deletedCategory
        };

        return result;
    }
}