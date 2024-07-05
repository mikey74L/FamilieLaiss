namespace FLBackEnd.API.GraphQL.Mutations.MediaItem;

[GraphQLDescription("The result for a new added media item")]
public class AddMediaItemPayload
{
    [GraphQLDescription("The new added media item")]
    public Domain.Entities.MediaItem MediaItem { get; set; } = default!;
}
