using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Linq;
using System.Threading.Tasks;
using UserInteraction.API.GraphQl.DataLoaders.Rating;
using UserInteraction.Infrastructure.DBContext;
using HotChocolate.Fusion.SourceSchema.Types;

namespace UserInteraction.API.GraphQl.Queries.Rating;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryRating
{
    [GraphQLDescription("Returns a list of ratings")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Domain.Aggregates.Rating> GetRatings(UserInteractionServiceDBContext context)
    {
        return context.Ratings;
    }
    
    [GraphQLDescription("Returns a rating")]
    [Lookup]
    public async Task<Domain.Aggregates.Rating> GetRating(long id, RatingDataLoader dataLoader)
        => await dataLoader.LoadAsync(id);
}