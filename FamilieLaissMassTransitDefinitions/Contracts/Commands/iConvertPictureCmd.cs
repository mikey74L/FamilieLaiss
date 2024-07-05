namespace FamilieLaissMassTransitDefinitions.Contracts.Commands;

/// <summary>
/// Convert photo command (Command-Class for MassTransit)
/// </summary>
public interface IConvertPictureCmd
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