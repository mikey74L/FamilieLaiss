using FLBackEnd.Infrastructure.DatabaseContext;

namespace FLBackEnd.API.GraphQL.Queries.UserSetting;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryUserSetting
{
    [GraphQLDescription("Returns a list of user settings")]
    [UseFiltering]
    public IQueryable<Domain.Entities.UserSetting> GetUserSettings(FamilieLaissDbContext context)
    {
        return context.UserSettings;
    }
}
