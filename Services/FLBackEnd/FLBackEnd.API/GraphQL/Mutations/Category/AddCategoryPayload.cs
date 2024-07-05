namespace FLBackEnd.API.GraphQL.Mutations.Category;

[GraphQLDescription("The result for a new added category")]
public class AddCategoryPayload
{
    [GraphQLDescription("The new added category")]
    public Domain.Entities.Category Category { get; set; } = default!;
}