using Google.API.Models;
using Microsoft.Extensions.Options;

namespace Google.API.GraphQl.Queries;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryGoogle
{
    [GraphQLDescription("Returns the api key for google maps")]
    public string GetGoogleMapsApiKey([Service] IOptions<AppSettings> appSettings)
    {
        return appSettings.Value.GoogleApiKey;
    }
}
