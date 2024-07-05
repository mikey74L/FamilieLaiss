using HotChocolate;
using HotChocolate.Types;
using User.Domain.Aggregates;
using User.Infrastructure.DBContext;

namespace User.API.GraphQL.Types
{
    public class UserType : ObjectType<User.Domain.Aggregates.User>
    {
        protected override void Configure(IObjectTypeDescriptor<User.Domain.Aggregates.User> descriptor)
        {
            descriptor.Field(p => p.Id)
                .Description("The identifier for this user");

            descriptor.Field(p => p.Country)
                .ResolveWith<Resolvers>(p => p.GetCountry(default!, default!))
                .UseDbContext<UserServiceDBContext>();

            descriptor.Field(p => p.CreateDate)
                .Description("When was this user created");

            descriptor.Field(p => p.ChangeDate)
                .Description("When was this user last changed");
        }

        private class Resolvers
        {
            public Country GetCountry([Parent] User.Domain.Aggregates.User user, [ScopedService] UserServiceDBContext context)
            {
                return context.Countries.Find(user.CountryID);
            }
        }
    }
}
