using Settings.API.GraphQL.Queries;
using Settings.Domain.Entities;
using Settings.Infrastructure.DBContext;

namespace User.API.GraphQL.Queries.UserSettings;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryUserSetting
{
    [GraphQLDescription("Returns a list of user settings")]
    [UseFiltering]
    public IQueryable<UserSetting> GetUserSettings(SettingsServiceDbContext context)
    {
        return context.UserSettings;
    }
}
