namespace FLBackEnd.API.GraphQL.Mutations.Category;

[GraphQLDescription("The result for a updated category")]
public class UpdateCategoryPayload
{
    [GraphQLDescription("The updated category")]
    public Domain.Entities.Category Category { get; set; } = default!;
}