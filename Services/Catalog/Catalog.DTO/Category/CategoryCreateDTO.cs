using FamilieLaissSharedObjects.Enums;
using System.ComponentModel.DataAnnotations;

namespace Catalog.DTO.Category;

/// <summary>
/// DTO-Class for creating a new category
/// </summary>
public class CategoryCreateDTO
{
    /// <summary>
    /// The type of category
    /// </summary>
    public EnumCategoryType CategoryType { get; set; }

    /// <summary>
    /// German name for this category
    /// </summary>
    [Required]
    [StringLength(300, MinimumLength = 1)]
    public string NameGerman { get; set; } = string.Empty;

    /// <summary>
    /// English name for this category
    /// </summary>
    [Required]
    [StringLength(300, MinimumLength = 1)]
    public string NameEnglish { get; set; } = string.Empty;
}
