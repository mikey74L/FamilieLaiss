namespace FamilieLaissMassTransitDefinitions.Contracts.Commands.UploadPicture;

/// <summary>
/// Convert photo command (Command-Class for MassTransit)
/// </summary>
public interface IMassConvertPictureCmd
{
    /// <summary>
    /// ID of upload item
    /// </summary>
    long Id { get; }

    /// <summary>
    /// ID of the convert status 
    /// </summary>
    long ConvertStatusId { get; }

    /// <summary>
    /// Original filename of uploaded file
    /// </summary>
    string OriginalName { get; }
}