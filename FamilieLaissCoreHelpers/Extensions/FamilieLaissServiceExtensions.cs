using FamilieLaissCoreHelpers.Interfaces;
using FamilieLaissCoreHelpers.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FamilieLaissCoreHelpers.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureWritableOptions<T>(
            this IServiceCollection services,
            IConfigurationSection section,
            string file = "appsettings.json") where T : class, new()
        {
            services.Configure<T>(section);
            services.AddTransient<IWritableOptions<T>>(provider =>
            {
                var options = provider.GetService<IOptionsMonitor<T>>();
                return new WritableOptions<T>(options, section.Key, file);
            });
        }
    }
}