using System.ComponentModel.DataAnnotations;

namespace Catalog.DTO.CategoryValue;

/// <summary>
/// DTO-Class for updating an existing category value
/// </summary>
public class CategoryValueUpdateDTO
{
    /// <summary>
    /// The unique identifier for the category value
    /// </summary>
    public long Id { get; set; }

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
