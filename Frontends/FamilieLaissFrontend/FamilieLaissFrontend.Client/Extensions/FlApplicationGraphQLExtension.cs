using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Models;

namespace FamilieLaissFrontend.Client.Extensions;

public static class FlApplicationGraphQlExtension
{
    public static IServiceCollection AddGraphQlClient(this IServiceCollection serviceCollection,
        IAppSettings? appSettings)
    {
        //serviceCollection.AddTransient<FlAuthorizationMessageHandler>();

        //serviceCollection.AddHttpClient(
        //    FamilieLaissClient.ClientName,
        //    client => client.BaseAddress = new Uri(appSettings?.UrlGraphQlHttp ?? ""))
        //.AddHttpMessageHandler<FlAuthorizationMessageHandler>();

        serviceCollection.AddFamilieLaissClient(profile: FamilieLaissClientProfileKind.Default)
            .ConfigureHttpClient((client) =>
            {
                client.BaseAddress = new Uri(appSettings?.UrlGraphQlHttp ?? "");
            })
            .ConfigureWebSocketClient((serviceProvider, client) =>
            {
                //var token = serviceProvider.GetRequiredService<ISomeService>().Token;

                client.Uri = new Uri(appSettings?.UrlGraphQlWs ?? "");
            });

        return serviceCollection;
    }
}