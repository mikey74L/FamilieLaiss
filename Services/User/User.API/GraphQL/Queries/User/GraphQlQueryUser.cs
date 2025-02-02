using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Fusion.SourceSchema.Types;
using HotChocolate.Types;
using System.Linq;
using System.Threading.Tasks;
using User.API.GraphQL.DataLoaders.UserAccount;
using User.Infrastructure.DBContext;

namespace User.API.GraphQL.Queries.UserQuery;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryUser
{
    [GraphQLDescription("Returns a list of users")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<User.Domain.Aggregates.UserAccount> GetUserAccounts(UserServiceDbContext context)
    {
        return context.UserAccounts;
    }

    [GraphQLDescription("Returns a user account")]
    [Lookup]
    public async Task<Domain.Aggregates.UserAccount> GetUserAccount(string id, UserAccountDataLoader dataLoader)
    => await dataLoader.LoadAsync(id);
}
