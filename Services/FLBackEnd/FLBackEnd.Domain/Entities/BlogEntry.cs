using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FLBackEnd.Domain.Entities;

/// <summary>
/// Entity for blog entry
/// </summary>
[GraphQLDescription("Blog item")]
public class BlogEntry : EntityModify<long>
{
    #region Properties

    /// <summary>
    /// German header text for this Blog-Entry
    /// </summary>
    [GraphQLDescription("German header for this blog item")]
    [GraphQLNonNullType]
    [MaxLength(200)]
    [Required]
    public string HeaderGerman { get; private set; } = string.Empty;

    /// <summary>
    /// English header text for this Blog-Entry
    /// </summary>
    [GraphQLDescription("English header for this blog item")]
    [GraphQLNonNullType]
    [MaxLength(200)]
    [Required]
    public string HeaderEnglish { get; private set; } = string.Empty;

    /// <summary>
    /// German text for this Blog-Entry
    /// </summary>
    [GraphQLDescription("German text for this blog item")]
    [GraphQLNonNullType]
    [MaxLength(50000)]
    [Required]
    public string TextGerman { get; private set; } = string.Empty;

    /// <summary>
    /// English text for this Blog-Entry
    /// </summary>
    [GraphQLDescription("English text for this blog item")]
    [GraphQLNonNullType]
    [MaxLength(50000)]
    [Required]
    public string TextEnglish { get; private set; } = string.Empty;

    #endregion

    #region C'tor

    private BlogEntry()
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="headerGerman">German header text for this Blog-Entry</param>
    /// <param name="headerEnglish">English header text for this Blog-Entry</param>
    /// <param name="textGerman">German text for this Blog-Entry</param>
    /// <param name="textEnglish">English text for this Blog-Entry</param>
    public BlogEntry(string headerGerman, string headerEnglish, string textGerman, string textEnglish)
    {
        if (string.IsNullOrEmpty(headerGerman))
        {
            throw new DomainException("A german header is required");
        }

        if (string.IsNullOrEmpty(headerEnglish))
        {
            throw new DomainException("A english header is required");
        }

        if (string.IsNullOrEmpty(textGerman))
        {
            throw new DomainException("A german text is required");
        }

        if (string.IsNullOrEmpty(textEnglish))
        {
            throw new DomainException("A english text is required");
        }

        HeaderGerman = headerGerman;
        HeaderEnglish = headerEnglish;
        TextGerman = textGerman;
        TextEnglish = textEnglish;
    }

    #endregion

    #region Domain Methods

    /// <summary>
    /// Update the blog entry
    /// </summary>
    /// <param name="headerGerman">German header text for this Blog-Entry</param>
    /// <param name="headerEnglish">English header text for this Blog-Entry</param>
    /// <param name="textGerman">German text for this Blog-Entry</param>
    /// <param name="textEnglish">English text for this Blog-Entry</param>
    [GraphQLIgnore]
    public void Update(string headerGerman, string headerEnglish, string textGerman, string textEnglish)
    {
        if (string.IsNullOrEmpty(headerGerman))
        {
            throw new DomainException("A german header is required");
        }

        if (string.IsNullOrEmpty(headerEnglish))
        {
            throw new DomainException("A english header is required");
        }

        if (string.IsNullOrEmpty(textGerman))
        {
            throw new DomainException("A german text is required");
        }

        if (string.IsNullOrEmpty(textEnglish))
        {
            throw new DomainException("A english text is required");
        }

        HeaderGerman = headerGerman;
        HeaderEnglish = headerEnglish;
        TextGerman = textGerman;
        TextEnglish = textEnglish;
    }

    #endregion

    #region Change-Tracker-Events

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