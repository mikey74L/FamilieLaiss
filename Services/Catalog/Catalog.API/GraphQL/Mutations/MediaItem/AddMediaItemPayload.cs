namespace Catalog.API.GraphQL.Mutations.MediaItem;

[GraphQLDescription("The result for a new added media item")]
public class AddMediaItemPayload
{
    [GraphQLDescription("The new added media item")]
    public Domain.Aggregates.MediaItem MediaItem { get; set; } = default!;
}