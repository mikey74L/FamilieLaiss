using DomainHelper.Interfaces;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Linq;
using System.Threading.Tasks;
using User.Infrastructure.DBContext;

namespace User.API.GraphQL.Queries.Country
{
    [ExtendObjectType(typeof(Query))]
    //[Authorize(Policy = "Country.Read")]
    public class QueryCountry
    {
        [GraphQLDescription("Returns a list of countries")]
        [UseDbContext(typeof(UserServiceDBContext))]
        [HotChocolate.Data.UseFiltering]
        [HotChocolate.Data.UseSorting]
        public IQueryable<User.Domain.Aggregates.Country> GetCountries([ScopedService] UserServiceDBContext context)
        {
            return context.Countries;
        }

        [GraphQLDescription("Returns a country")]
        public async Task<User.Domain.Aggregates.Country> GetCountryAsync([Service] iUnitOfWork unitOfWork, string id)
        {
            var repository = unitOfWork.GetReadOnlyRepository<User.Domain.Aggregates.Country>();

            var item = await repository.GetOneAsync(id);

            return item;
        }
    }
}
