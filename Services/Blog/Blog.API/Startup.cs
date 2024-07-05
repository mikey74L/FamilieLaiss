using Blog.API.Models;
using Blog.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Blog.API
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
                    //Ermitteln des Datenbankservice
                    var Factory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<BlogServiceDBContext>>();
                    var ServiceToExecute = await Factory.CreateDbContextAsync();

                    //Eine Retry-Policy mit Polly erstellen.
                    //Falls beim Start des Containers der zugehörige Datenbankcontainer noch nicht bereit sein sollte
                    var RetryPolicy = Policy.Handle<Exception>()
                        .WaitAndRetry(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

                    //Starten der Migration über die Retry-Policy
                    await RetryPolicy.Execute(async () =>
                    {
                        await ServiceToExecute.Database.MigrateAsync();
                        await ServiceToExecute.Database.CloseConnectionAsync();
                        await ServiceToExecute.DisposeAsync();
                    });
                }
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
