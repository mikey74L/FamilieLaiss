using FamilieLaissInterfaces;
using FamilieLaissServices;

namespace FamilieLaissFrontend.Client.Extensions;

public static class FlApplicationHelperExtension
{
    public static IServiceCollection AddGlobalFunctions(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IGlobalFunctions, GlobalFunctionsService>();

        return serviceCollection;
    }
}