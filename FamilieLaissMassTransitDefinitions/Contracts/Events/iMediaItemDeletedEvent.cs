namespace FamilieLaissMassTransitDefinitions.Contracts.Events
{
    public interface iMediaItemDeletedEvent
    {
        long ID { get; }

        bool IsPicture { get; }

        bool DeleteUploadItem { get; }

        long UploadItemID { get; }
    }
}
