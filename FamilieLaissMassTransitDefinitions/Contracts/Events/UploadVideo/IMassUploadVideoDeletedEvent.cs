namespace FamilieLaissMassTransitDefinitions.Contracts.Events.UploadVideo;

/// <summary>
/// Upload video deleted event (Event-Class for MassTransit)
/// </summary>
public interface IMassUploadVideoDeletedEvent
{
    /// <summary>
    /// Identifier for upload video
    /// </summary>
    long Id { get; }
}