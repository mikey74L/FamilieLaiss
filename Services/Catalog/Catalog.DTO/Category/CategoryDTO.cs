using Catalog.DTO.CategoryValue;
using FamilieLaissSharedObjects.Enums;
using System;
using System.Collections.Generic;

namespace Catalog.DTO.Category;

/// <summary>
/// DTO-Class for querying a category
/// </summary>
public class CategoryDTO
{
    /// <summary>
    /// The unique identifier for the category
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The type of category
    /// </summary>
    public EnumCategoryType? CategoryType { get; set; }

    /// <summary>
    /// German name for this category
    /// </summary>
    public string? NameGerman { get; set; } = string.Empty;

    /// <summary>
    /// English name for this category
    /// </summary>
    public string? NameEnglish { get; set; } = string.Empty;

    /// <summary>
    /// List of related category values
    /// </summary>
    public IEnumerable<CategoryValueDTO>? CategoryValues { get; set; }

    /// <summary>
    /// The date and time when the category was created
    /// </summary>
    public DateTimeOffset? CreateDate { get; set; }

    /// <summary>
    /// The date and time when the category was last changed
    /// </summary>
    public DateTimeOffset? ChangeDate { get; set; }
}
