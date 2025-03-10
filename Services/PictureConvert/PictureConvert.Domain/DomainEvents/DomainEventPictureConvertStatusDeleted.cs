using DomainHelper.DomainEvents;

namespace PictureConvert.Domain.DomainEvents;

/// <summary>
/// Event for upload picture converted
/// </summary>
public class DomainEventPictureConvertStatusDeleted : DomainEventSingle
{
    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for upload picture</param>
    public DomainEventPictureConvertStatusDeleted(long id) : base(id.ToString())
    {
    }

    #endregion
}