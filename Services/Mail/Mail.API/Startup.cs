using Mail.API.Models;
using Mail.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Mail.API
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
                using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

                //Ermitteln der DB-Factory
                var factory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<MailServiceDBContext>>();

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
        #endregion

        #region MassTransit EndpointConventions
        public static void ConfigureEndpointConventions(AppSettings appSettings)
        {
            //Setzen der Sending-Endpoint-Mappings
            //EndpointConvention.Map<iConvertPictureCmd>(new Uri("queue:" + appSettings.Endpoint_PictureConverterServiceExecute));
        }
        #endregion
    }
}
