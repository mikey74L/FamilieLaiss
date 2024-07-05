namespace FamilieLaissMassTransitDefinitions.Contracts.Events
{
    /// <summary>
    /// Upload video deleted event (Event-Class for MassTransit)
    /// </summary>
    public interface iUploadVideoDeletedEvent
    {
        /// <summary>
        /// The primary key for upload video
        /// </summary>
        long ID { get; }
    }
}
