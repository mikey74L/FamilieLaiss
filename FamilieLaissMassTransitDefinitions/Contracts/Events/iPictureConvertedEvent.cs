namespace FamilieLaissMassTransitDefinitions.Contracts.Events;

public interface IPictureConvertedEvent
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