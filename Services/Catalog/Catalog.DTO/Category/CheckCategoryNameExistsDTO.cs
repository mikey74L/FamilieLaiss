using FamilieLaissSharedObjects.Enums;

namespace Catalog.DTO.Category;

public class CheckCategoryNameExistsDTO
{
    /// <summary>
    /// The unique identifier for an existing category or -1 if new
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The category type for the category to check
    /// </summary>
    public EnumCategoryType CategoryType { get; set; }

    /// <summary>
    /// The name to check
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
