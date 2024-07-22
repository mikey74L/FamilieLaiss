namespace Catalog.API.GraphQL.Mutations.Category;

[GraphQLDescription("The result for a deleted category")]
public class DeleteCategoryPayload
{
    [GraphQLDescription("The deleted category")]
    public Domain.Aggregates.Category Category { get; set; } = default!;
}