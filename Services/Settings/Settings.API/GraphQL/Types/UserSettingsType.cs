using Settings.Domain.Entities;

namespace Settings.API.GraphQL.Types
{
    public class UserSettingsType : ObjectType<UserSettings>
    {
        protected override void Configure(IObjectTypeDescriptor<UserSettings> descriptor)
        {
            descriptor.Field(p => p.Id)
                .Description("The identifier for this user setting");

            //descriptor.Field(p => p.Users)
            //    .ResolveWith<Resolvers>(p => p.GetUsers(default!, default!))
            //    .UseDbContext<UserServiceDBContext>();

            descriptor.Field(p => p.CreateDate)
                .Description("When was this user setting added to database");

            descriptor.Field(p => p.ChangeDate)
                .Description("When was this user setting the last time changed in database");
        }

        private class Resolvers
        {
            //public IQueryable<User.Domain.Aggregates.User> GetUsers([Parent] Country country, [ScopedService] UserServiceDBContext context)
            //{
            //    return context.Users.Where(x => x.CountryID == country.Id);
            //}
        }
    }
}
