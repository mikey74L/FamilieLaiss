namespace Catalog.API.GraphQL.Mutations.MediaItem;

public class RemoveMediaItemPayload
{
    public Catalog.Domain.Aggregates.MediaItem MediaItem { get; set; }
}
