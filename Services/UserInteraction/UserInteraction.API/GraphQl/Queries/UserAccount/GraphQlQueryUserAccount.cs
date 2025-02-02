using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Fusion.SourceSchema.Types;
using HotChocolate.Types;
using UserInteraction.API.GraphQl.DataLoaders.UserAccount;

namespace UserInteraction.API.GraphQl.Queries.UserAccount;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryUserAccount
{
    [GraphQLDescription("Returns a user account")]
    [Lookup]
    public async Task<Domain.Aggregates.UserAccount> GetUserAccount(string id, UserAccountDataLoader dataLoader)
        => await dataLoader.LoadAsync(id);
}