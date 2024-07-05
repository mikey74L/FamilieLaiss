using System;
using System.Threading.Tasks;
using InfrastructureHelper.EventDispatchHandler;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PictureConvertExecuteService.Interfaces;
using PictureConvertExecuteService.MassTransit.Consumers;
using PictureConvertExecuteService.Models;
using PictureConvertExecuteService.Services;
using Serilog;
using ServiceHelper.Interfaces;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;

namespace PictureConvertExecuteService;

public static class ConsumerExtension
{
    public static void ConfigureConsumers(this IServiceCollection services)
    {
        services.AddTransient<ConvertPictureConsumer>();
    }
}

public class Program
{
    #region Private Static Methods

    private static void ConfigureEndpointConventions(AppSettings appSettings)
    {
    }

    #endregion

    private static async Task Main(string[] args)
    {
        string currentEnvironment = Environment.GetEnvironmentVariable("Environment");

        if (currentEnvironment != null)
            await Host.CreateDefaultBuilder(args)
                .AddServiceDiscovery(options => options.UseEureka())
                .UseEnvironment(currentEnvironment)
                .ConfigureLogging(logging => logging.ClearProviders())
                .UseSerilog((hostingContext, _, loggerConfiguration) =>
                    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration))
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<ConsoleHostedService>();

                    services.AddHostedService<EventDispatcherBackgroundService>();

                    services.AddTransient<IMetaExtractor, MetaExtractorService>();
                    services.AddTransient<IConvertPicture, ConvertPictureService>();
                    services.AddTransient<IPictureInfoExtractor, PictureInfoExtractorService>();
                    services.AddTransient<IJobExecutor, JobExecutorService>();
                    services.AddTransient<IPreconditions, PreconditionsService>();
                    services.AddTransient<IDatabaseOperations, DatabaseOperationsService>();
                    services.AddSingleton<iEventStore, EventStoreService>();

                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

                    services.ConfigureConsumers();

                    var currentAppSettings = hostContext.Configuration.GetSection("AppSettings").Get<AppSettings>();
                    ConfigureEndpointConventions(currentAppSettings);

                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<ConvertPictureConsumer>();

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            LogContext.ConfigureCurrentLogContext(context.GetRequiredService<ILoggerFactory>());

                            cfg.Host(new Uri(currentAppSettings.RabbitMqConnection));

                            cfg.ReceiveEndpoint(currentAppSettings.EndpointNameExecutor, e =>
                            {
                                e.UseConcurrencyLimit(1);
                                e.PrefetchCount = 16;
                                e.UseMessageRetry(r =>
                                    r.Incremental(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20)));

                                e.ConfigureConsumer<ConvertPictureConsumer>(context);
                            });
                        });
                    });

                    services.Configure<MassTransitHostOptions>(options =>
                    {
                        options.WaitUntilStarted = true;
                        options.StartTimeout = TimeSpan.FromMinutes(2);
                        options.StopTimeout = TimeSpan.FromMinutes(1);
                    });

                    services.Configure<AppSettings>(hostContext.Configuration.GetSection("AppSettings"));

                    services.Configure<ConnectionStrings>(hostContext.Configuration.GetSection("ConnectionStrings"));
                })
                .RunConsoleAsync();
    }
}