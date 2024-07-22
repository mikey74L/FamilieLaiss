using FamilieLaissSharedObjects.Enums;

namespace Catalog.API.GraphQL.Mutations.Category;

[GraphQLDescription("InputData type for adding categories")]
public class AddCategoryInput
{
    [GraphQLDescription("The type of category")]
    public EnumCategoryType CategoryType { get; set; }

    [GraphQLDescription("German name for this category")]
    public string NameGerman { get; set; } = string.Empty;

    [GraphQLDescription("English name for this category")]
    public string NameEnglish { get; set; } = string.Empty;
}