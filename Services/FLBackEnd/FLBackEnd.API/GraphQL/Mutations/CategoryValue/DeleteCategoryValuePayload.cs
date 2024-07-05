namespace FLBackEnd.API.GraphQL.Mutations.CategoryValue;

[GraphQLDescription("The result for a deleted category value")]
public class DeleteCategoryValuePayload
{
    [GraphQLDescription("The deleted category value")]
    public Domain.Entities.CategoryValue CategoryValue { get; set; } = default!;
}