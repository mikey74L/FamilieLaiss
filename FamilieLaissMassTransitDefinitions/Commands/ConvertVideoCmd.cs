using FamilieLaissMassTransitDefinitions.Contracts.Commands;

namespace FamilieLaissMassTransitDefinitions.Commands;

/// <summary>
/// Convert photo command (Command-Class for MassTransit)
/// </summary>
public class ConvertVideoCmd : IConvertVideoCmd
{
    /// <summary>
    /// ID of upload item
    /// </summary>
    public required long Id { get; init; }

    /// <summary>
    /// ID of the convert status 
    /// </summary>
    public required long ConvertStatusId { get; init; }

    /// <summary>
    /// Original filename of uploaded file
    /// </summary>
    public required string OriginalName { get; init; }
}