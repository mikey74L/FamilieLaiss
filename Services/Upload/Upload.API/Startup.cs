using Microsoft.EntityFrameworkCore;
using Polly;
using Upload.API.Models;
using Upload.Infrastructure.DBContext;

namespace Upload.API;

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

            var factory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<UploadServiceDbContext>>();

            var dbContext = factory.CreateDbContext();

            //Eine Retry-Policy mit Polly erstellen.
            //Falls beim Start des Containers der zugehörige Datenbankcontainer noch nicht bereit sein sollte
            var retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetry(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            retryPolicy.Execute(dbContext.Database.Migrate);

            dbContext.Database.CloseConnection();
            dbContext.Dispose();
        }
    }

    #endregion

    #region MassTransit EndpointConventions

    public static void ConfigureEndpointConventions(AppSettings appSettings)
    {
        //Setzen der Sending-Endpoint-Mappings
        //EndpointConvention.Map<iCreateMessageForUserGroupCmd>(new Uri("queue:" + appSettings.Endpoint_MessageService));
    }

    #endregion
}