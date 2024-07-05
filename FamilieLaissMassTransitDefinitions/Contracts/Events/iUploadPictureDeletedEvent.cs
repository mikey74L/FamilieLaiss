namespace FamilieLaissMassTransitDefinitions.Contracts.Events
{
    /// <summary>
    /// Upload picture deleted event (Event-Class for MassTransit)
    /// </summary>
    public interface iUploadPictureDeletedEvent
    {
        /// <summary>
        /// The primary key for upload picture
        /// </summary>
        long ID { get; }
    }
}
