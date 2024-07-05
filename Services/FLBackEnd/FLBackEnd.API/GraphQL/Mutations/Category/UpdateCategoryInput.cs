namespace FLBackEnd.API.GraphQL.Mutations.Category;

[GraphQLDescription("InputData type for updating categories")]
public class UpdateCategoryInput
{
    [GraphQLDescription("The ID for the category to update")]
    public long Id { get; set; }

    [GraphQLDescription("German name for this category")]
    public string NameGerman { get; set; } = string.Empty;

    [GraphQLDescription("English name for this category")]
    public string NameEnglish { get; set; } = string.Empty;
}