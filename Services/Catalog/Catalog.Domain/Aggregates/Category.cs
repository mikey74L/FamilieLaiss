using Catalog.Domain.DomainEvents.Category;
using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using FamilieLaissSharedObjects.Enums;
using HotChocolate;
using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Catalog.Domain.Aggregates;

/// <summary>
/// Entity for category
/// </summary>
[GraphQLDescription("Category group")]
public class Category : EntityModify<long>
{
    #region Private Properties

    private readonly ILazyLoader _lazyLoader;

    #endregion

    #region Properties

    /// <summary>
    /// The type of category
    /// </summary>
    [Required]
    [GraphQLDescription("The type of category")]
    public EnumCategoryType CategoryType { get; private set; }

    /// <summary>
    /// German name for this category
    /// </summary>
    [Required]
    [MaxLength(300)]
    [GraphQLDescription("German name for this category")]
    [GraphQLNonNullType]
    public string NameGerman { get; private set; } = string.Empty;

    /// <summary>
    /// English name for this category
    /// </summary>
    [Required]
    [MaxLength(300)]
    [GraphQLDescription("English name for this category")]
    [GraphQLNonNullType]
    public string NameEnglish { get; private set; } = string.Empty;

    /// <summary>
    /// List of related category values
    /// </summary>
    [GraphQLDescription("List of related category values")]
    [UseFiltering]
    [UseSorting]
    public ICollection<CategoryValue> CategoryValues { get; private set; } = [];

    #endregion

    #region C'tor

    /// <summary>
    /// Default constructor for GraphQL
    /// </summary>
    private Category()
    {
    }

    /// <summary>
    /// Constructor would be used by EF-Core
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    private Category(ILazyLoader lazyLoader)
    {
        _lazyLoader = lazyLoader;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="categoryType">The type of category</param>
    /// <param name="nameGerman">German name for this category</param>
    /// <param name="nameEnglish">English name for this category</param>
    public Category(EnumCategoryType? categoryType, string? nameGerman, string? nameEnglish)
    {
        if (categoryType is null)
        {
            throw new DomainException(DomainExceptionType.WrongParameter, "A german name is needed for this category");
        }

        if (string.IsNullOrEmpty(nameGerman))
        {
            throw new DomainException(DomainExceptionType.WrongParameter, "A german name is needed for this category");
        }

        if (string.IsNullOrEmpty(nameEnglish))
        {
            throw new DomainException(DomainExceptionType.WrongParameter, "A english name is needed for this category");
        }

        CategoryType = categoryType.Value;
        NameGerman = nameGerman;
        NameEnglish = nameEnglish;

        CategoryValues = [];
    }

    #endregion

    #region Domain Methods

    /// <summary>
    /// Update the category
    /// </summary>
    /// <param name="nameGerman">German name for this category</param>
    /// <param name="nameEnglish">English name for this category</param>
    [GraphQLIgnore]
    public void Update(string? nameGerman, string? nameEnglish)
    {
        if (string.IsNullOrEmpty(nameGerman))
        {
            throw new DomainException(DomainExceptionType.WrongParameter, "A german name is needed for this category");
        }

        if (string.IsNullOrEmpty(nameEnglish))
        {
            throw new DomainException(DomainExceptionType.WrongParameter, "A english name is needed for this category");
        }

        NameGerman = nameGerman;
        NameEnglish = nameEnglish;
    }

    /// <summary>
    /// Add a value to a category
    /// </summary>
    /// <param name="germanName">The german name for the category value</param>
    /// <param name="englishName">The english name for this category value</param>
    /// <returns>The added category value</returns>
    [GraphQLIgnore]
    public CategoryValue AddCategoryValue(string? germanName, string? englishName)
    {
        var valueEntity = new CategoryValue(this, germanName, englishName);

        CategoryValues.Add(valueEntity);

        return valueEntity;
    }

    /// <summary>
    /// Remove a value from this category
    /// </summary>
    /// <param name="categoryValueId">The Identifier for category value</param>
    [GraphQLIgnore]
    public async Task RemoveCategoryValue(long categoryValueId)
    {
        bool itemFound = false;

        await _lazyLoader.LoadAsync(this, navigationName: nameof(CategoryValues));

        foreach (var item in CategoryValues)
        {
            if (item.Id == categoryValueId)
            {
                itemFound = true;
                CategoryValues.Remove(item);
                break;
            }
        }

        if (!itemFound)
        {
            throw new DomainException(DomainExceptionType.NoDataFound);
        }
    }

    #endregion

    #region Called from Change-Tracker

    [GraphQLIgnore]
    public override Task EntityModifiedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        AddDomainEvent(new DomainEventCategoryChanged(Id));

        return Task.CompletedTask;
    }

    [GraphQLIgnore]
    public override Task EntityAddedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        AddDomainEvent(new DomainEventCategoryCreated(Id));

        return Task.CompletedTask;
    }

    [GraphQLIgnore]
    public override async Task EntityDeletedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        AddDomainEvent(new DomainEventCategoryDeleted(Id));

        await _lazyLoader.LoadAsync(this, navigationName: nameof(CategoryValues));

        foreach (var item in CategoryValues)
        {
            await item.EntityDeletedAsync(dbContext, dictContextParams);
        }
    }

    #endregion
}