using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using FlBackEnd.API.Models;
using FLBackEnd.Infrastructure.DatabaseContext;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace FlBackEnd.API;

/// <summary>
/// Startup-Class for ASP.NET Core
/// </summary>
public static class Startup
{
    #region Migrate Database for Service

    public static void InitializeDatabase(IApplicationBuilder app)
    {
        var serviceScopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();

        if (serviceScopeFactory is not null)
        {
            using var serviceScope = serviceScopeFactory.CreateScope();

            var factory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<FamilieLaissDbContext>>();

            var dbContext = factory.CreateDbContext();

            var retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetry(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            retryPolicy.Execute(dbContext.Database.Migrate);

            dbContext.Database.CloseConnection();
            dbContext.Dispose();
        }
    }

    #endregion

    #region MassTransit EndpointConventions

    public static void ConfigureEndpointConventions(AppSettings? appSettings)
    {
        //Set endpoint mappings for MassTransit
        if (appSettings is not null)
        {
            EndpointConvention.Map<IMassConvertPictureCmd>(
                new Uri("queue:" + appSettings.EndpointPictureConvertService));
            EndpointConvention.Map<IMassConvertVideoCmd>(new Uri("queue:" + appSettings.EndpointVideoConvertService));
        }
    }

    #endregion
}