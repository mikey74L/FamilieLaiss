using FamilieLaissMassTransitDefinitions.Contracts.Events;

namespace FamilieLaissMassTransitDefinitions.Events;

public class PictureConvertedEvent : IPictureConvertedEvent
{
    #region Interface iPictureConvertedEvent
    public required long UploadPictureId { get; init; }

    public required long ConvertStatusId { get; init; }
    #endregion
}
