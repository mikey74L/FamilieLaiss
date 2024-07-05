namespace FLBackEnd.API.GraphQL.Mutations.MediaItem;

[GraphQLDescription("The result for a deleted media item")]
public class DeleteMediaItemPayload
{
    [GraphQLDescription("The deleted media item")]
    public Domain.Entities.MediaItem MediaItem { get; set; } = default!;
}
