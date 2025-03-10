using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Linq;
using System.Threading.Tasks;
using UserInteraction.API.GraphQl.DataLoaders.UserInteraction;
using UserInteraction.Infrastructure.DBContext;
using HotChocolate.Fusion.SourceSchema.Types;

namespace UserInteraction.API.GraphQl.Queries.UserInteraction;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryUserInteraction
{
    [GraphQLDescription("Returns a list of user interaction infos")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Domain.Aggregates.UserInteractionInfo> GetUserInteractionInfos(UserInteractionServiceDBContext context)
    {
        return context.UserInteractionInfos;
    }
    
    [GraphQLDescription("Returns a user interaction info")]
    [Lookup]
    public async Task<Domain.Aggregates.UserInteractionInfo> GetUserInteractionInfo(long id, UserInteractionDataLoader dataLoader)
        => await dataLoader.LoadAsync(id);
}