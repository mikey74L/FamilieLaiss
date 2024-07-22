namespace FamilieLaissMassTransitDefinitions.Contracts.Events;

public interface IMassPictureConvertedEvent
{
    /// <summary>
    /// ID of upload picture
    /// </summary>
    long UploadPictureId { get; }

    /// <summary>
    /// ID for status entry
    /// </summary>
    long ConvertStatusId { get; }
}