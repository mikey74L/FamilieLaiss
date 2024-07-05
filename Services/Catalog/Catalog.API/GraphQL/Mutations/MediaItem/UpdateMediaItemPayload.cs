namespace Catalog.API.GraphQL.Mutations.MediaItem;

public class UpdateMediaItemPayload
{
    public Catalog.Domain.Aggregates.MediaItem MediaItem { get; set; }
}
