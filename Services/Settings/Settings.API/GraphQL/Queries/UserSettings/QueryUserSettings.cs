using Settings.API.GraphQL.Queries;
using Settings.Infrastructure.DBContext;

namespace User.API.GraphQL.Queries.UserSettings
{
    [ExtendObjectType(typeof(Query))]
    public class QueryUserSettings
    {
        [GraphQLDescription("Returns a list of user settings")]
        [HotChocolate.Data.UseFiltering]
        [HotChocolate.Data.UseSorting]
        public IQueryable<Settings.Domain.Entities.UserSettings> GetUserSettings(SettingsServiceDBContext context)
        {
            return context.UserSettings;
        }
    }
}
