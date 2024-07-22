using Catalog.API.Mediator.Commands.CategoryValue;
using MediatR;

namespace Catalog.API.GraphQL.Mutations.CategoryValue;

[ExtendObjectType(typeof(Mutation))]
public class GraphQlMutationCategoryValue
{
    [GraphQLDescription("Add a new category value")]
    public async Task<AddCategoryValuePayload> AddCategoryValueAsync(AddCategoryValueInput input,
        [Service] IMediator mediator)
    {
        var newCategoryValue = await mediator.Send(new MtrAddCategoryValueCommand() { InputData = input });

        var result = new AddCategoryValuePayload
        {
            CategoryValue = newCategoryValue
        };

        return result;
    }

    [GraphQLDescription("Update existing category value")]
    public async Task<UpdateCategoryValuePayload> UpdateCategoryValueAsync(UpdateCategoryValueInput input,
        [Service] IMediator mediator)
    {
        var updatedCategoryValueValue = await mediator.Send(new MtrUpdateCategoryValueCommand() { InputData = input });

        var result = new UpdateCategoryValuePayload
        {
            CategoryValue = updatedCategoryValueValue
        };

        return result;
    }

    [GraphQLDescription("Delete existing category value")]
    public async Task<DeleteCategoryValuePayload> DeleteCategoryValueAsync(DeleteCategoryValueInput input,
        [Service] IMediator mediator)
    {
        var deletedCategoryValue = await mediator.Send(new MtrDeleteCategoryValueCommand() { InputData = input });

        var result = new DeleteCategoryValuePayload
        {
            CategoryValue = deletedCategoryValue
        };

        return result;
    }
}