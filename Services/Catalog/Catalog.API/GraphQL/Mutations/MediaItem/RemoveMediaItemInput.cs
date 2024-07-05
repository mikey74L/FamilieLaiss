namespace Catalog.API.GraphQL.Mutations.MediaItem;

public class RemoveMediaItemInput
{
    public long MediaGroupId { get; set; }

    public long MediaItemId { get; set; }

    public bool DeleteUploadItem { get; set; }
}
