namespace FamilieLaissMassTransitDefinitions.Contracts.Commands
{
    /// <summary>
    /// Convert video command (Command-Class for MassTransit)
    /// </summary>
    public interface IConvertVideoCmd
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
}
