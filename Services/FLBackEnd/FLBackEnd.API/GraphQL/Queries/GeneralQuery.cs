using FlBackEnd.API.Models;
using Microsoft.Extensions.Options;

namespace FLBackEnd.API.GraphQL.Queries;

[ExtendObjectType(typeof(Query))]
public class GeneralQuery
{
    [GraphQLDescription("Returns the api key for google maps")]
    public string GetGoogleMapsApiKey([Service] IOptions<AppSettings> appSettings)
    {
        return appSettings.Value.GoogleMapsApiKey;
    }
}
