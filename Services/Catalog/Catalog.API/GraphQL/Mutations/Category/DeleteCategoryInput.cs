namespace Catalog.API.GraphQL.Mutations.Category;

[GraphQLDescription("InputData type for deleting categories")]
public class DeleteCategoryInput
{
    [GraphQLDescription("The Id for the category")]
    public long Id { get; set; }
}