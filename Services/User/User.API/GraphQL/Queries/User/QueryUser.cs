using DomainHelper.Interfaces;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Linq;
using System.Threading.Tasks;
using User.Infrastructure.DBContext;

namespace User.API.GraphQL.Queries.UserQuery
{
    [ExtendObjectType(typeof(Query))]
    //[Authorize(Policy = "User.Read")]
    public class QueryUser
    {
        [GraphQLDescription("Returns a list of users")]
        [UseDbContext(typeof(UserServiceDBContext))]
        [HotChocolate.Data.UseFiltering]
        [HotChocolate.Data.UseSorting]
        public IQueryable<User.Domain.Aggregates.User> GetUsers([ScopedService] UserServiceDBContext context)
        {
            return context.Users;
        }

        [GraphQLDescription("Returns a user by id")]
        public async Task<User.Domain.Aggregates.User> GetUserByIdAsync([Service] iUnitOfWork unitOfWork, string id)
        {
            var repository = unitOfWork.GetReadOnlyRepository<User.Domain.Aggregates.User>();

            var item = await repository.GetOneAsync(id);

            return item;
        }
    }
}
