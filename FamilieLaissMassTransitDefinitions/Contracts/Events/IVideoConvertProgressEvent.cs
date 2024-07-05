namespace FamilieLaissMassTransitDefinitions.Contracts.Events;

public interface IVideoConvertProgressEvent
{
    /// <summary>
    /// ID of upload video
    /// </summary>
    long UploadVideoId { get; }

    /// <summary>
    /// ID for status entry
    /// </summary>
    long ConvertStatusId { get; }   
}
