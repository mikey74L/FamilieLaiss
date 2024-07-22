namespace Catalog.API.GraphQL.Mutations.MediaItem;

[GraphQLDescription("InputData type for deleting media item")]
public class DeleteMediaItemInput
{
    [GraphQLDescription("The ID for the media group the media item belongs to")]
    public long MediaGroupId { get; private set; }

    [GraphQLDescription("The ID for the media item to delete")]
    public long MediaItemId { get; private set; }

    [GraphQLDescription("Should assigned upload item be kept in the upload area?")]
    public bool KeepUploadItem { get; set; }
}