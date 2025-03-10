using DomainHelper.AbstractClasses;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PictureConvert.Domain.Entities;

/// <summary>
/// Entity for representing the upload picture
/// </summary>
public class UploadPicture : EntityBase<long>
{
    #region Properties
    /// <summary>
    /// The original filename of the upload picture with file extension
    /// </summary>
    [Required]
    [MaxLength(255)]
    [GraphQLIgnore]
    public string Filename { get; private set; } = string.Empty;

    /// <summary>
    /// The converting status for this upload picture
    /// </summary>
    [GraphQLDescription("The converting status for this upload picture")]
    public PictureConvertStatus? Status { get; private set; }
    #endregion

    #region C'tor

    /// <summary>
    /// Default constructor for GraphQL
    /// </summary>
    private UploadPicture()
    {
    }

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">The ID for the upload picture</param>
    /// <param name="filename">The original filename for the upload picture with extension</param>
    public UploadPicture(long id, string filename)
    {
        Id = id;
        Filename = filename;
    }

    #endregion

    #region Called from Change-Tracker

    public override Task EntityAddedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        return Task.CompletedTask;
    }

    public override Task EntityDeletedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        return Task.CompletedTask;
    }

    #endregion
}