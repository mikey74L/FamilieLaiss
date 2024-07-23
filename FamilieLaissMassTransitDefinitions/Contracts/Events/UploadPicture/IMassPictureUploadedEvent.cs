namespace FamilieLaissMassTransitDefinitions.Contracts.Events.UploadPicture;

/// <summary>
/// Picture uploaded event (Event-Class for MassTransit)
/// </summary>
public interface IMassPictureUploadedEvent
{
    /// <summary>
    /// Identifier for upload picture
    /// </summary>
    long Id { get; }

    /// <summary>
    /// The filename for the uploaded picture
    /// </summary>
    string Filename { get; }
}