using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Fusion.SourceSchema.Types;
using HotChocolate.Types;
using System.Linq;
using System.Threading.Tasks;
using User.API.GraphQL.DataLoaders.Country;
using User.Infrastructure.DBContext;

namespace User.API.GraphQL.Queries.Country;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryCountry
{
    [GraphQLDescription("Returns a list of countries")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<User.Domain.Aggregates.Country> GetCountries(UserServiceDbContext context)
    {
        return context.Countries;
    }

    [GraphQLDescription("Returns a country")]
    [Lookup]
    public async Task<Domain.Aggregates.Country> GetCountry(string id, CountryDataLoader dataLoader)
    => await dataLoader.LoadAsync(id);
}
