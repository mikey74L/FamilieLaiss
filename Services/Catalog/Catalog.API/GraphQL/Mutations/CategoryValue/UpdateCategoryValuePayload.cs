namespace Catalog.API.GraphQL.Mutations.CategoryValue;

[GraphQLDescription("The result for a updated category value")]
public class UpdateCategoryValuePayload
{
    [GraphQLDescription("The updated category value")]
    public Domain.Aggregates.CategoryValue CategoryValue { get; set; } = default!;
}