using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FLBackEnd.Domain.Entities;

/// <summary>
/// Entity for category value
/// </summary>
[GraphQLDescription("Category values")]
public class CategoryValue : EntityModify<long>
{
    #region Properties

    /// <summary>
    /// Identifier for the category
    /// </summary>
    [GraphQLIgnore]
    [Required]
    public long CategoryId { get; private set; }

    /// <summary>
    /// The Category this category value entry belongs to
    /// </summary>
    [GraphQLDescription("The Category this category value entry belongs to")]
    public Category Category { get; private set; } = default!;

    /// <summary>
    /// German name for this category value
    /// </summary>
    [Required]
    [MaxLength(300)]
    [GraphQLDescription("German name for this category value")]
    [GraphQLNonNullType]
    public string NameGerman { get; private set; } = string.Empty;

    /// <summary>
    /// English name for this category value
    /// </summary>
    [Required]
    [MaxLength(300)]
    [GraphQLDescription("English name for this category value")]
    [GraphQLNonNullType]
    public string NameEnglish { get; private set; } = string.Empty;

    /// <summary>
    /// List of related media item category values
    /// </summary>
    [GraphQLDescription("List of related media item category values")]
    [UseFiltering]
    [UseSorting]
    public ICollection<MediaItemCategoryValue> MediaItemCategoryValues { get; private set; } = [];
    #endregion

    #region C'tor

    /// <summary>
    /// Constructor without parameters would be used by EF-Core and GraphQL
    /// </summary>
    protected CategoryValue()
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="category">The category for this value</param>
    /// <param name="nameGerman">The german name for this value</param>
    /// <param name="nameEnglish">The english name for this value</param>
    internal CategoryValue(Category category, string? nameGerman, string? nameEnglish)
    {
        if (string.IsNullOrEmpty(nameGerman))
        {
            throw new DomainException("A german name is needed for this category value");
        }

        if (string.IsNullOrEmpty(nameEnglish))
        {
            throw new DomainException("A english name is needed for this category value");
        }

        Category = category;
        NameGerman = nameGerman;
        NameEnglish = nameEnglish;
    }

    #endregion

    #region Domain-Methods

    /// <summary>
    /// Updates the category value data
    /// </summary>
    /// <param name="nameGerman">The german name for category value</param>
    /// <param name="nameEnglish">The english name for category value</param>
    [GraphQLIgnore]
    public void Update(string? nameGerman, string? nameEnglish)
    {
        if (string.IsNullOrEmpty(nameGerman))
        {
            throw new DomainException("A german name is needed for this category value");
        }

        if (string.IsNullOrEmpty(nameEnglish))
        {
            throw new DomainException("A english name is needed for this category value");
        }

        NameGerman = nameGerman;
        NameEnglish = nameEnglish;
    }

    #endregion

    #region Called from Change-Tracker

    [GraphQLIgnore]
    public override Task EntityModifiedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        return Task.CompletedTask;
    }

    [GraphQLIgnore]
    public override Task EntityAddedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        return Task.CompletedTask;
    }

    [GraphQLIgnore]
    public override Task EntityDeletedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        return Task.CompletedTask;
    }

    #endregion
}