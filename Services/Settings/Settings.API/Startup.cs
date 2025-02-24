using Microsoft.EntityFrameworkCore;
using Polly;
using Settings.API.Models;
using Settings.Domain.Entities;
using Settings.Infrastructure.DBContext;

namespace Settings.API;

/// <summary>
/// Startup-Class for ASP.NET Core
/// </summary>
public class Startup
{
    #region Migrate Database for Service
    public static void InitializeDatabase(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

        //Ermitteln der DB-Factory
        var factory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<SettingsServiceDbContext>>();

        //Ermitteln des DB-Contexts aus der Factory
        using var dbContext = factory.CreateDbContext();

        //Eine Retry-Policy mit Polly erstellen.
        //Falls beim Start des Containers der zugehörige Datenbankcontainer noch nicht bereit sein sollte
        var retryPolicy = Policy.Handle<Exception>()
            .WaitAndRetry(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        //Starten der Migration über die Retry-Policy
        retryPolicy.Execute(dbContext.Database.Migrate);

        //Freigeben des DBContexts
        dbContext.Database.CloseConnection();
    }

    public static void SeedDatabase(IApplicationBuilder app)
    {
        //Ermitteln des Scopes für DI
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

        //Ermitteln der DB-Factory
        var factory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<SettingsServiceDbContext>>();

        //Ermitteln des DB-Contexts aus der Factory
        using var dbContext = factory.CreateDbContext();

        //Eine Retry-Policy mit Polly erstellen.
        //Falls beim Start des Containers der zugehörige Datenbankcontainer noch nicht bereit sein sollte
        var retryPolicy = Policy.Handle<Exception>()
            .WaitAndRetry(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        //Starten des Seeden der Administratoren über die Retry-Policy
        retryPolicy.Execute(() =>
        {
            if (dbContext.UserSettings.Count() == 0)
            {
                var settingsMla = new UserSetting("61c3d1fa0ff95f00680126e8");
                settingsMla.SetCreateDate();
                var settingsKla = new UserSetting("61c3d2bb1c2b8d0069dda985");
                settingsKla.SetCreateDate();

                dbContext.UserSettings.Add(settingsMla);
                dbContext.UserSettings.Add(settingsKla);

                dbContext.SaveChanges();
            }
        });

        //Freigeben des DBContexts
        dbContext.Database.CloseConnection();
    }
    #endregion

    #region MassTransit EndpointConventions
    public static void ConfigureEndpointConventions(AppSettings? appSettings)
    {
        //Setzen der Sending-Endpoint-Mappings
        //EndpointConvention.Map<iConvertPictureCmd>(new Uri("queue:" + appSettings.Endpoint_PictureConverterServiceExecute));
    }
    #endregion
}
