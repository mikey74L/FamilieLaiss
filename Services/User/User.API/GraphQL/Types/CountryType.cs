using HotChocolate;
using HotChocolate.Types;
using System.Linq;
using User.Domain.Aggregates;
using User.Infrastructure.DBContext;

namespace User.API.GraphQL.Types
{
    public class CountryType : ObjectType<Country>
    {
        protected override void Configure(IObjectTypeDescriptor<Country> descriptor)
        {
            descriptor.Field(p => p.Id)
                .Description("The identifier for this country");

            descriptor.Field(p => p.Users)
                .ResolveWith<Resolvers>(p => p.GetUsers(default!, default!))
                .UseDbContext<UserServiceDBContext>();

            descriptor.Field(p => p.CreateDate)
                .Description("When was this country added to database");
        }

        private class Resolvers
        {
            public IQueryable<User.Domain.Aggregates.User> GetUsers([Parent] Country country, [ScopedService] UserServiceDBContext context)
            {
                return context.Users.Where(x => x.CountryID == country.Id);
            }
        }
    }
}
