using Catalog.Domain.Aggregates;
using DomainHelper.AbstractClasses;
using System.Threading.Tasks;

namespace Catalog.Domain.Entities;

/// <summary>
/// Entity for representing the upload video
/// </summary>
public class UploadVideo : EntityBase<long>
{
    #region Properties
    /// <summary>
    /// The original filename of the upload video with file extension
    /// </summary>
    public string Filename { get; private set; }

    public long? MediaItemID { get; private set; }

    public MediaItem? MediaItem { get; private set; }
    #endregion

    #region C'tor
    /// <summary>
    /// C'tor without parameters would be used by EF-Core
    /// </summary>
    private UploadVideo()
    {

    }

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">The ID for the upload video</param>
    /// <param name="filename">The original filename for the upload video with extension</param>
    public UploadVideo(long id, string filename)
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
