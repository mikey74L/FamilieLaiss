namespace FamilieLaissMassTransitDefinitions.Contracts.Events;

/// <summary>
/// Upload picture deleted event (Event-Class for MassTransit)
/// </summary>
public interface IMassUploadPictureDeletedEvent
{
    /// <summary>
    /// Identifier for upload picture
    /// </summary>
    long Id { get; }
}