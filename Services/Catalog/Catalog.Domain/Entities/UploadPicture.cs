using Catalog.Domain.Aggregates;
using DomainHelper.AbstractClasses;
using System.Threading.Tasks;

namespace Catalog.Domain.Entities;

/// <summary>
/// Entity for representing the upload picture
/// </summary>
public class UploadPicture : EntityCreation<long>
{
    #region Properties
    /// <summary>
    /// The original filename of the upload picture with file extension
    /// </summary>
    public string Filename { get; private set; }

    public long? MediaItemID { get; private set; }

    public MediaItem? MediaItem { get; private set; }
    #endregion

    #region C'tor
    /// <summary>
    /// C'tor without parameters would be used by EF-Core
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
    public override Task EntityAddedAsync()
    {
        return Task.CompletedTask;
    }

    public override Task EntityDeletedAsync()
    {
        return Task.CompletedTask;
    }
    #endregion
}
