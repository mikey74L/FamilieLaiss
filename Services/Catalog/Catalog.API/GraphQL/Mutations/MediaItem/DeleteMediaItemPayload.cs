namespace Catalog.API.GraphQL.Mutations.MediaItem;

[GraphQLDescription("The result for a deleted media item")]
public class DeleteMediaItemPayload
{
    [GraphQLDescription("The deleted media item")]
    public Domain.Aggregates.MediaItem MediaItem { get; set; } = default!;
}