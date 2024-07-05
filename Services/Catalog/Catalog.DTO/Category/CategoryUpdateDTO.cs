using System.ComponentModel.DataAnnotations;

namespace Catalog.DTO.Category;

/// <summary>
/// DTO-Class for updating an existing category
/// </summary>
public class CategoryUpdateDTO
{
    /// <summary>
    /// The unique identifier for the category
    /// </summary>
    public long Id { get; set; }

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
