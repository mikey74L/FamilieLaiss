using DomainHelper.DomainEvents;

namespace VideoConvert.Domain.DomainEvents;

/// <summary>
/// Event for upload video converted
/// </summary>
public class DomainEventVideoConvertStatusDeleted : DomainEventSingle
{
    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for upload video</param>
    public DomainEventVideoConvertStatusDeleted(long id) : base(id.ToString())
    {
    }

    #endregion
}