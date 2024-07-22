using FamilieLaissMassTransitDefinitions.Contracts.Events;

namespace FamilieLaissMassTransitDefinitions.Events;

public class MassMediaItemDeletedEvent : IMassMediaItemDeletedEvent
{
    #region Properties

    /// <summary>
    /// Identifier for media item
    /// </summary>
    public long Id { get; private set; }

    /// <summary>
    /// Identifier for assigned upload item
    /// </summary>
    public long UploadItemId { get; private set; }

    /// <summary>
    /// Should upload item be deleted
    /// </summary>
    public bool DeleteUploadItem { get; private set; }

    /// <summary>
    /// Is media item a picture
    /// </summary>
    public bool IsPicture { get; private set; }

    #endregion

    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for media item</param>
    /// <param name="uploadItemId">Identifier for upload item</param>
    /// <param name="isPicture">Is media item a picture</param>
    /// <param name="deleteUploadItem">Should upload item be deleted</param>
    public MassMediaItemDeletedEvent(long id, long uploadItemId, bool isPicture, bool deleteUploadItem)
    {
        Id = id;
        UploadItemId = uploadItemId;
        IsPicture = isPicture;
        DeleteUploadItem = deleteUploadItem;
    }

    #endregion
}