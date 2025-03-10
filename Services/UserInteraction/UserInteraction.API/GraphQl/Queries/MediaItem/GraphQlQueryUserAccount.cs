using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Fusion.SourceSchema.Types;
using HotChocolate.Types;
using UserInteraction.API.GraphQl.DataLoaders.MediaItem;

namespace UserInteraction.API.GraphQl.Queries.MediaItem;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryMediaItem
{
    [GraphQLDescription("Returns a media item")]
    [Lookup]
    public async Task<Domain.Aggregates.MediaItem> GetMediaItem(long id, MediaItemDataLoader dataLoader)
        => await dataLoader.LoadAsync(id);
}