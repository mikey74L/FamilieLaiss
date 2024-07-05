namespace FLBackEnd.API.GraphQL.Mutations.CategoryValue;

[GraphQLDescription("The result for a updated category value")]
public class UpdateCategoryValuePayload
{
    [GraphQLDescription("The updated category value")]
    public Domain.Entities.CategoryValue CategoryValue { get; set; } = default!;
}