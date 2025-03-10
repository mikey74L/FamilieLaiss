namespace Catalog.API.GraphQL.Mutations.CategoryValue;

[GraphQLDescription("The result for a new added category value")]
public class AddCategoryValuePayload
{
    [GraphQLDescription("The new added category value")]
    public Domain.Aggregates.CategoryValue CategoryValue { get; set; } = default!;
}