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
        Task.Run(async () =>
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();
            
            var factory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<UserInteractionServiceDBContext>>();

            var dbContext = await factory.CreateDbContextAsync();

            var retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetry(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            await retryPolicy.Execute(async () =>
            {
                await dbContext.Database.MigrateAsync();
            });
        });
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
