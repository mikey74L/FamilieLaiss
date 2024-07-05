namespace FamilieLaissMassTransitDefinitions.Contracts.Events
{
    /// <summary>
    /// Upload Picture assigned event (Event-Class for MassTransit)
    /// </summary>
    public interface iUploadPictureAssignedEvent
    {
        /// <summary>
        /// The primary key for upload picture
        /// </summary>
        long ID { get; }
    }
}
