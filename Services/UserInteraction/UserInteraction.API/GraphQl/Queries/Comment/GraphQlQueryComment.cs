using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Fusion.SourceSchema.Types;
using HotChocolate.Types;
using System.Linq;
using System.Threading.Tasks;
using UserInteraction.API.GraphQl.DataLoaders.Comment;
using UserInteraction.Infrastructure.DBContext;

namespace UserInteraction.API.GraphQl.Queries.Comment;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryComment
{
    [GraphQLDescription("Returns a list of comments")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Domain.Aggregates.Comment> GetComments(UserInteractionServiceDBContext context)
    {
        return context.Comments;
    }

    [GraphQLDescription("Returns a comment")]
    [Lookup]
    public async Task<Domain.Aggregates.Comment> GetComment(long id, CommentDataLoader dataLoader)
    => await dataLoader.LoadAsync(id);
}