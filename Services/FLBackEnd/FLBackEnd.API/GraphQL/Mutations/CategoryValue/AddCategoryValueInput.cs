namespace FLBackEnd.API.GraphQL.Mutations.CategoryValue;

[GraphQLDescription("Input type for adding category value")]
public class AddCategoryValueInput
{
    [GraphQLDescription("The key for the category")]
    public long CategoryId { get; set; }

    [GraphQLDescription("German name for this category value")]
    public string NameGerman { get; set; } = string.Empty;

    [GraphQLDescription("English name for this category value")]
    public string NameEnglish { get; set; } = string.Empty;
}