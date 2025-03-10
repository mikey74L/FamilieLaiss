using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Threading.Tasks;
using UserInteraction.API.Models;
using UserInteraction.Infrastructure.DBContext;

namespace UserInteraction.API;

/// <summary>
/// Startup-Class for ASP.NET Core
/// </summary>
public static class Startup
{
    #region Migrate Database for Service
    public static void InitializeDatabase(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

        //Ermitteln der DB-Factory
        var factory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<UserInteractionServiceDBContext>>();

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
    #endregion

    #region MassTransit EndpointConventions
    public static void ConfigureEndpointConventions(AppSettings? appSettings)
    {
        if (appSettings is not null)
        {
            //EndpointConvention.Map<iResetPasswordRequestCmd>(new Uri("queue:" + appSettings.Endpoint_IdentityService)); 
        }
    }
    #endregion
}
