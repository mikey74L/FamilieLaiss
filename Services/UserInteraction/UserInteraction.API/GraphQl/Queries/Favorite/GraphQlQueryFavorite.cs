using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Linq;
using System.Threading.Tasks;
using UserInteraction.API.GraphQl.DataLoaders.Favorite;
using UserInteraction.Infrastructure.DBContext;
using HotChocolate.Fusion.SourceSchema.Types;

namespace UserInteraction.API.GraphQl.Queries.Favorite;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryFavorite
{
    [GraphQLDescription("Returns a list of favorites")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Domain.Aggregates.Favorite> GetFavorites(UserInteractionServiceDBContext context)
    {
        return context.Favorites;
    }
    
    [GraphQLDescription("Returns a favorite")]
    [Lookup]
    public async Task<Domain.Aggregates.Favorite> GetFavorite(long id, FavoriteDataLoader dataLoader)
        => await dataLoader.LoadAsync(id);
}