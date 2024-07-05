namespace FLBackEnd.API.GraphQL.Mutations.CategoryValue;

[GraphQLDescription("Input type for updating category values")]
public class UpdateCategoryValueInput
{
    [GraphQLDescription("The ID for the category value to update")]
    public long Id { get; set; }

    [GraphQLDescription("German name for this category value")]
    public string NameGerman { get; set; } = string.Empty;

    [GraphQLDescription("English name for this category value")]
    public string NameEnglish { get; set; } = string.Empty;
}