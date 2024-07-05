using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace FamilieLaissFrontend.Client.Extensions;

public static class FlApplicationAuthorizationExtension
{
    public static IServiceCollection AddAuthFamilieLaiss(this IServiceCollection serviceCollection,
        WebAssemblyHostBuilder builder)
    {
        //serviceCollection.AddOidcAuthentication(options =>
        //{
        //    builder.Configuration.Bind("Auth0", options.ProviderOptions);
        //    options.ProviderOptions.ResponseType = "code";
        //});

        return serviceCollection;
    }
}