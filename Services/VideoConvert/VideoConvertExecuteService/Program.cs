using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using InfrastructureHelper.EventDispatchHandler;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using ServiceHelper.Interfaces;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;
using System;
using System.Threading.Tasks;
using VideoConvertExecuteService.Interfaces;
using VideoConvertExecuteService.MassTransit.Consumer;
using VideoConvertExecuteService.Models;
using VideoConvertExecuteService.Services;

namespace VideoConvertExecuteService;

public class Program
{
    #region Private Static Methods

    private static void ConfigureConsumers(IServiceCollection services, IBusRegistrationConfigurator conf)
    {
        services.AddTransient<ConvertVideoConsumer>();
        conf.AddConsumer<ConvertVideoConsumer>();
    }

    private static void ConfigureEndpointConventions(AppSettings appSettings)
    {
        //Set endpoint mappings for MassTransit
        if (appSettings is not null)
        {
            EndpointConvention.Map<IMassSetVideoInfoDataCmd>(
                new Uri("queue:" + appSettings.EndpointNameUploadService));
        }
    }

    #endregion

    private static async Task Main(string[] args)
    {
        string currentEnvironment = Environment.GetEnvironmentVariable("Environment");

        if (currentEnvironment != null)
        {
            await Host.CreateDefaultBuilder(args)
                .AddServiceDiscovery(options => options.UseEureka())
                .UseEnvironment(currentEnvironment)
                .ConfigureLogging(logging => logging.ClearProviders())
                .UseSerilog((hostingContext, _, loggerConfiguration) =>
                    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration))
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<ConsoleHostedService>();

                    services.AddTransient<IMetadataExtractor, MetadataExtractorService>();
                    services.AddTransient<IVideoConverter, VideoConverterService>();
                    services.AddTransient<IJobExecutor, JobExecutorService>();
                    services.AddTransient<IPreconditions, PreconditionsService>();
                    services.AddTransient<IDatabaseOperations, DatabaseOperationsService>();
                    services.AddTransient<IConvertPicture, ConvertPictureService>();
                    services.AddSingleton<iEventStore, EventStoreService>();

                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

                    var currentAppSettings = hostContext.Configuration.GetSection("AppSettings").Get<AppSettings>();
                    ConfigureEndpointConventions(currentAppSettings);

                    services.AddMassTransit(x =>
                    {
                        ConfigureConsumers(services, x);

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

                                e.ConfigureConsumer<ConvertVideoConsumer>(context);
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
}