﻿using FamilieLaissSharedUI.Enums;
using FamilieLaissSharedUI.Interfaces;
using FamilieLaissSharedUI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace FamilieLaissSharedUI.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddMvvmNavigation(this IServiceCollection services, LibraryConfiguration? configuration = null)
    {
        if (configuration is not null)
        {
            switch (configuration.HostingModel)
            {
                case BlazorHostingModel.WebAssembly:
                case BlazorHostingModel.Hybrid:
                case BlazorHostingModel.NotSpecified:
                    services.AddSingleton<IMvvmNavigationManager, MvvmNavigationManager>();
                    break;
                case BlazorHostingModel.Server:
                case BlazorHostingModel.WebApp:
                    services.AddScoped<IMvvmNavigationManager, MvvmNavigationManager>();
                    break;
            }
        }
        else
        {
            services.AddSingleton<IMvvmNavigationManager, MvvmNavigationManager>();
        }
        return services;
    }

    public static IServiceCollection AddMvvmNavigation(this IServiceCollection services, Action<LibraryConfiguration> configuration)
    {
        LibraryConfiguration options = new();
        configuration.Invoke(options);

        return AddMvvmNavigation(services, options);
    }
}