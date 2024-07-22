namespace FamilieLaissMassTransitDefinitions.Contracts.Events;

/// <summary>
/// Picture uploaded event (Event-Class for MassTransit)
/// </summary>
public interface IMassVideoUploadedEvent
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