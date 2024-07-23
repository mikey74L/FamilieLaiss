using Microsoft.EntityFrameworkCore;
using Polly;
using Settings.API.Models;
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
        Task.Run(async () =>
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

            //Ermitteln der DB-Factory
            var factory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<SettingsServiceDbContext>>();

            //Ermitteln des DB-Contexts aus der Factory
            var dbContext = await factory.CreateDbContextAsync();

            //Eine Retry-Policy mit Polly erstellen.
            //Falls beim Start des Containers der zugehörige Datenbankcontainer noch nicht bereit sein sollte
            var retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetry(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            //Starten der Migration über die Retry-Policy
            await retryPolicy.Execute(async () =>
            {
                await dbContext.Database.MigrateAsync();
            });

            //Freigeben des DBContexts
            await dbContext.Database.CloseConnectionAsync();
            await dbContext.DisposeAsync();
        });
    }

    public static void SeedDatabase(IApplicationBuilder app)
    {
        Task.Run(async () =>
        {
            //Ermitteln des Scopes für DI
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

            //Ermitteln der DB-Factory
            var factory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<SettingsServiceDbContext>>();

            //Ermitteln des DB-Contexts aus der Factory
            var dbContext = await factory.CreateDbContextAsync();

            //Eine Retry-Policy mit Polly erstellen.
            //Falls beim Start des Containers der zugehörige Datenbankcontainer noch nicht bereit sein sollte
            var retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetry(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            //Starten des Seeden der Administratoren über die Retry-Policy
            //await retryPolicy.Execute(async () =>
            //{
            //    if (await dbContext.UserSettings.CountAsync() == 0)
            //    {
            //        var settingsMla = new UserSettings("61c3d1fa0ff95f00680126e8");
            //        var settingsKla = new UserSettings("61c3d2bb1c2b8d0069dda985");

            //        await dbContext.UserSettings.AddAsync(settingsMla);
            //        await dbContext.UserSettings.AddAsync(settingsKla);

            //        await dbContext.SaveChangesAsync();
            //    }
            //});

            //Freigeben des DBContexts
            await dbContext.Database.CloseConnectionAsync();
            await dbContext.DisposeAsync();
        });
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
