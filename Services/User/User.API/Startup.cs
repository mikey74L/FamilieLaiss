using FamilieLaissMassTransitDefinitions.Contracts.Commands.User;
using FamilieLaissSharedObjects.Enums;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Linq;
using User.API.Helper;
using User.API.Models;
using User.Infrastructure.DBContext;

namespace User.API;

/// <summary>
/// Startup-Class for ASP.NET Core
/// </summary>
public static class Startup
{
    #region Migrate and Seed Database for Service
    public static void InitializeDatabase(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

        //Ermitteln der DB-Factory
        var factory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<UserServiceDbContext>>();

        //Ermitteln des DB-Contexts aus der Factory
        var dbContext = factory.CreateDbContext();

        //Eine Retry-Policy mit Polly erstellen.
        //Falls beim Start des Containers der zugehörige Datenbankcontainer noch nicht bereit sein sollte
        var retryPolicy = Policy.Handle<Exception>()
            .WaitAndRetry(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        //Starten der Migration über die Retry-Policy
        retryPolicy.Execute(() =>
        {
            dbContext.Database.Migrate();
        });

        //Freigeben des DBContexts
        dbContext.Database.CloseConnection();
        dbContext.Dispose();
    }

    public static void SeedDatabase(IApplicationBuilder app)
    {
        //Ermitteln des Scopes für DI
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

        //Ermitteln der DB-Factory
        var factory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<UserServiceDbContext>>();

        //Ermitteln des DB-Contexts aus der Factory
        var dbContext = factory.CreateDbContext();

        //Eine Retry-Policy mit Polly erstellen.
        //Falls beim Start des Containers der zugehörige Datenbankcontainer noch nicht bereit sein sollte
        var retryPolicy = Policy.Handle<Exception>()
            .WaitAndRetry(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        //Starten des Seeden der Countries über die Retry-Policy
        retryPolicy.Execute(() =>
        {
            if (dbContext.Countries.Count() == 0)
            {
                dbContext.Countries.AddRange(CountryList.GetCountryList());
                foreach (var country in dbContext.Countries)
                {
                    country.SetCreateDate();
                }

                dbContext.SaveChanges();
            }
        });

        //Starten des Seeden der Administratoren über die Retry-Policy
        retryPolicy.Execute(() =>
        {
            if (dbContext.UserAccounts.Count() == 0)
            {
                var adminUser1 = new Domain.Aggregates.UserAccount("61c3d1fa0ff95f00680126e8", "mlaiss", "mikey74@hotmail.de",
                    EnumGenderType.Male, "Michael", "Laiß", "Goldmühlestraße", "6", "71065", "Sindelfingen", "DE");
                adminUser1.SetCreateDate();
                var adminUser2 = new Domain.Aggregates.UserAccount("61c3d2bb1c2b8d0069dda985", "klaiss", "klaudija@klaudija-s.de",
                     EnumGenderType.Female, "Klaudija", "Laiß", "Goldmühlestraße", "6", "71065", "Sindelfingen", "DE");
                adminUser2.SetCreateDate();
                dbContext.UserAccounts.Add(adminUser1);
                dbContext.UserAccounts.Add(adminUser2);

                dbContext.SaveChanges();
            }
        });

        //Freigeben des DBContexts
        dbContext.Database.CloseConnection();
        dbContext.Dispose();
    }
    #endregion

    #region MassTransit EndpointConventions
    public static void ConfigureEndpointConventions(AppSettings? appSettings)
    {
        //Deklaration
        EndpointConvention.Map<IMassCreateUserSettingsCmd>(
            new Uri("queue:" + appSettings?.EndpointSettingsService));
    }
    #endregion
}
