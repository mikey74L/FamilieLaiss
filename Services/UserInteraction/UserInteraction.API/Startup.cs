using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Threading.Tasks;
using UserInteraction.API.Models;
using UserInteraction.Infrastructure.DBContext;

namespace UserInteraction.API
{
    /// <summary>
    /// Startup-Class for ASP.NET Core
    /// </summary>
    public static class Startup
    {
        #region Migrate Database for Service
        public static void InitializeDatabase(IApplicationBuilder app)
        {
            Task.Run(async () =>
            {
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope())
                {
                    //Ermitteln der DB-Factory
                    var factory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<UserInteractionServiceDBContext>>();

                    //Ermitteln des DB-Contexts aus der Factory
                    var dbContext = await factory.CreateDbContextAsync();

                    //Eine Retry-Policy mit Polly erstellen.
                    //Falls beim Start des Containers der zugehörige Datenbankcontainer noch nicht bereit sein sollte
                    var RetryPolicy = Policy.Handle<Exception>()
                        .WaitAndRetry(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

                    //Starten der Migration über die Retry-Policy
                    await RetryPolicy.Execute(async () =>
                    {
                        await dbContext.Database.MigrateAsync();
                    });
                }
            });
        }
        #endregion

        #region MassTransit EndpointConventions
        public static void ConfigureEndpointConventions(AppSettings appSettings)
        {
            //Setzen der Sending-Endpoint-Mappings
            EndpointConvention.Map<iResetPasswordRequestCmd>(new Uri("queue:" + appSettings.Endpoint_IdentityService));
        }
        #endregion
    }
}
