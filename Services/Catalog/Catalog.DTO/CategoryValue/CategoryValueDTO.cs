using System;

namespace Catalog.DTO.CategoryValue;

/// <summary>
/// DTO-Class for querying a category value
/// </summary>
public class CategoryValueDTO
{
    /// <summary>
    /// The unique identifier for the category value
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The category id this category value entry belongs to
    /// </summary>
    public long CategoryId { get; set; }

    /// <summary>
    /// German name for this category value
    /// </summary>
    public string? NameGerman { get; set; }

    /// <summary>
    /// English name for this category value
    /// </summary>
    public string? NameEnglish { get; set; }

    /// <summary>
    /// The date and time when the category value was created
    /// </summary>
    public DateTimeOffset? CreateDate { get; set; }

    /// <summary>
    /// The date and time when the category value was last changed
    /// </summary>
    public DateTimeOffset? ChangeDate { get; set; }
}
