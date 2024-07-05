using System.ComponentModel.DataAnnotations;

namespace Catalog.DTO.CategoryValue;

/// <summary>
/// DTO-Class for creating a new category value
/// </summary>
public class CategoryValueCreateDTO
{
    /// <summary>
    /// The category id this category value entry belongs to
    /// </summary>
    public long CategoryId { get; set; }

    /// <summary>
    /// German name for this category value
    /// </summary>
    [Required]
    [StringLength(300, MinimumLength = 1)]
    public string NameGerman { get; set; } = string.Empty;

    /// <summary>
    /// English name for this category value
    /// </summary>
    [Required]
    [StringLength(300, MinimumLength = 1)]
    public string NameEnglish { get; set; } = string.Empty;

}
