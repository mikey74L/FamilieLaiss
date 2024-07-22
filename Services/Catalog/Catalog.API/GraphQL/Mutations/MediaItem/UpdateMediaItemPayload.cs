namespace Catalog.API.GraphQL.Mutations.MediaItem;

[GraphQLDescription("The result for a updated media item")]
public class UpdateMediaItemPayload
{
    [GraphQLDescription("The updated media item")]
    public Domain.Aggregates.MediaItem MediaItem { get; set; } = default!;
}