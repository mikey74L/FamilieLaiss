namespace FLBackEnd.API.GraphQL.Mutations.MediaItem;

[GraphQLDescription("The result for a updated media item")]
public class UpdateMediaItemPayload
{
    [GraphQLDescription("The updated media item")]
    public Domain.Entities.MediaItem MediaItem { get; set; } = default!;
}
