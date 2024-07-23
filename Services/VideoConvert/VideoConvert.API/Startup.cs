using FamilieLaissMassTransitDefinitions.Contracts.Commands.UploadVideo;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Polly;
using VideoConvert.API.Models;
using VideoConvert.Infrastructure.DBContext;

namespace VideoConvert.API;

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
        var factory = serviceScope.ServiceProvider
            .GetRequiredService<IDbContextFactory<VideoConvertServiceDbContext>>();

        //Ermitteln des DB-Contexts aus der Factory
        var dbContext = factory.CreateDbContext();

        //Eine Retry-Policy mit Polly erstellen.
        //Falls beim Start des Containers der zugehörige Datenbankcontainer noch nicht bereit sein sollte
        var retryPolicy = Policy.Handle<Exception>()
            .WaitAndRetry(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        //Starten der Migration über die Retry-Policy
        retryPolicy.Execute(() => { dbContext.Database.Migrate(); });
    }

    #endregion

    #region MassTransit EndpointConventions

    public static void ConfigureEndpointConventions(AppSettings appSettings)
    {
        //Setzen der Sending-Endpoint-Mappings
        EndpointConvention.Map<IMassConvertVideoCmd>(
            new Uri("queue:" + appSettings.EndpointVideoConverterServiceExecute));
    }

    #endregion
}