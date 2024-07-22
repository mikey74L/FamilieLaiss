namespace Catalog.API.GraphQL.Mutations.Category;

[GraphQLDescription("The result for a updated category")]
public class UpdateCategoryPayload
{
    [GraphQLDescription("The updated category")]
    public Domain.Aggregates.Category Category { get; set; } = default!;
}