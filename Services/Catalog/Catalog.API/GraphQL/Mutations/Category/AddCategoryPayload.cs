namespace Catalog.API.GraphQL.Mutations.Category;

[GraphQLDescription("The result for a new added category")]
public class AddCategoryPayload
{
    [GraphQLDescription("The new added category")]
    public Domain.Aggregates.Category Category { get; set; } = default!;
}