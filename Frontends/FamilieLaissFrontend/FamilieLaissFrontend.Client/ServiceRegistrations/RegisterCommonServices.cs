using FamilieLaissFrontend.Client.Extensions;
using FamilieLaissInterfaces.Models;
using FamilieLaissSharedUI.Enums;
using FamilieLaissSharedUI.Extensions;
using MudBlazor.Services;
using MudExtensions.Services;

namespace FamilieLaissFrontend.Client.ServiceRegistrations;

public static class CommonServiceHelper
{
    public static void ConfigureCommonServices(this IServiceCollection services, IAppSettings? appSettings)
    {
        services.AddMvvmNavigation(options =>
        {
            options.HostingModel = BlazorHostingModel.WebApp;
        });

        services.AddGraphQlClient(appSettings);

        services.AddViewModels();

        services.AddAppServices();

        services.AddFluentValidators();

        services.AddGlobalFunctions();

        services.AddMudServices(o =>
        {
            o.SnackbarConfiguration.ShowCloseIcon = true;
            o.SnackbarConfiguration.ClearAfterNavigation = false;
            o.SnackbarConfiguration.NewestOnTop = true;
            o.SnackbarConfiguration.PreventDuplicates = false;
        });

        services.AddMudExtensions();

        services.AddLocalization(x => x.ResourcesPath = "Resources");
    }
}