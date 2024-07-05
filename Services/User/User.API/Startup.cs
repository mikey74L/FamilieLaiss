using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Threading.Tasks;
using User.API.Helper;
using User.API.Models;
using User.Infrastructure.DBContext;

namespace User.API
{
    /// <summary>
    /// Startup-Class for ASP.NET Core
    /// </summary>
    public static class Startup
    {
        #region Migrate and Seed Database for Service
        public static void InitializeDatabase(IApplicationBuilder app)
        {
            Task.Run(async () =>
            {
                using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

                //Ermitteln der DB-Factory
                var factory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<UserServiceDBContext>>();

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
                var factory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<UserServiceDBContext>>();

                //Ermitteln des DB-Contexts aus der Factory
                var dbContext = await factory.CreateDbContextAsync();

                //Eine Retry-Policy mit Polly erstellen.
                //Falls beim Start des Containers der zugehörige Datenbankcontainer noch nicht bereit sein sollte
                var retryPolicy = Policy.Handle<Exception>()
                    .WaitAndRetry(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

                //Starten des Seeden der Countries über die Retry-Policy
                await retryPolicy.Execute(async () =>
                {
                    if (await dbContext.Countries.CountAsync() == 0)
                    {
                        await dbContext.Countries.AddRangeAsync(CountryList.GetCountryList());

                        await dbContext.SaveChangesAsync();
                    }
                });

                //Starten des Seeden der Administratoren über die Retry-Policy
                await retryPolicy.Execute(async () =>
                {
                    if (await dbContext.Users.CountAsync() == 0)
                    {
                        var AdminUser1 = new Domain.Aggregates.User("61c3d1fa0ff95f00680126e8", "mikey74@hotmail.de", "Michael Laiß");
                        var AdminUser2 = new Domain.Aggregates.User("61c3d2bb1c2b8d0069dda985", "klaudija@klaudija-s.de", "Klaudija Laiß");
                        await dbContext.Users.AddAsync(AdminUser1);
                        await dbContext.Users.AddAsync(AdminUser2);

                        await dbContext.SaveChangesAsync();
                    }
                });

                //Freigeben des DBContexts
                await dbContext.Database.CloseConnectionAsync();
                await dbContext.DisposeAsync();
            });
        }
        #endregion

        #region MassTransit EndpointConventions
        public static void ConfigureEndpointConventions(AppSettings appSettings)
        {
            //Deklaration
            //string rabbitMqServerUrl = appSettings.RabbitMQServer_URL;
        }
        #endregion
    }
}
