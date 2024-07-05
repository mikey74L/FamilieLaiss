using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Google.API.Swagger;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo()
        {
            Title = "FamilieLaiss - Google.API",
            Version = description.ApiVersion.ToString(),
            Description = "Die Google.API für die Onlinepräsenz der Familie Laiß",
            TermsOfService = new Uri("https://www.familielaiss.de/TermsOfService"),
            Contact = new OpenApiContact()
            {
                Name = "Michael Laiss",
                Email = "laiss.michael@gmail.com",
                Url = new Uri("https://www.familielaiss.de")
            },
            License = new OpenApiLicense()
            {
                Name = "GNU General Public License",
                Url = new Uri("https://www.gnu.org/licenses/gpl.html")
            }
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }

        return info;
    }
}