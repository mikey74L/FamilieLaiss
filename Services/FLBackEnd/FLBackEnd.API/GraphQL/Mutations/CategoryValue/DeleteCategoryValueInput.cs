namespace FLBackEnd.API.GraphQL.Mutations.CategoryValue;

[GraphQLDescription("Input type for deleting category values")]
public class DeleteCategoryValueInput
{
    [GraphQLDescription("The ID for the category value")]
    public long Id { get; set; }
}