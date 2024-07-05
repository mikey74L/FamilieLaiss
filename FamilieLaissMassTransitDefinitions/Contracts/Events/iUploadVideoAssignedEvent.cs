namespace FamilieLaissMassTransitDefinitions.Contracts.Events
{
    /// <summary>
    /// Upload video assigned event (Event-Class for MassTransit)
    /// </summary>
    public interface iUploadVideoAssignedEvent
    {
        /// <summary>
        /// The primary key for upload video
        /// </summary>
        long ID { get; }
    }
}
