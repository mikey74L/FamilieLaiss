using InfrastructureHelper.EventDispatchHandler;
using MassTransit;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using ServiceLayerHelper;
using ServiceLayerHelper.Logging;
using Settings.API;
using Settings.API.GraphQL.DataLoaders;
using Settings.API.GraphQL.Mutations;
using Settings.API.GraphQL.Mutations.UserSettings;
using Settings.API.GraphQL.Queries;
using Settings.API.GraphQL.Types;
using Settings.API.Logging;
using Settings.API.Models;
using Settings.Infrastructure.DBContext;
using Steeltoe.Discovery.Client;
using System.Globalization;
using User.API.GraphQL.Queries.UserSettings;

Console.Title = "Settings-Service";

var builder = WebApplication.CreateBuilder(args);

//Integrate Aspire
builder
    .AddServiceDefaults()
    .AddNpgsqlDbContext<SettingsServiceDbContext>("settings-db",
        settings => settings.DisableRetry = true);

//Logging
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Host.UseSerilog(logger);

//Create the bootstrap logger for Serilog
Log.Logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .CreateBootstrapLogger();

//Adding an HTTPContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//Adding the Hosted-Services (Background-Services)
builder.Services.AddHostedService<EventDispatcherBackgroundService>();

//Adding the global exception handler middleware
builder.Services.AddSingleton<ILog, LogSerilog>();

//Adding configuration (App-Settings) to the IOC container
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
AppSettings? appSettings = appSettingsSection.Get<AppSettings>();

//Adding pooled context factory and add unit of work
builder.Services.AddPooledDbContextFactory<SettingsServiceDbContext>(options =>
    {
    })
    .AddUnitOfWork<SettingsServiceDbContext>();

//Den GraphQL-Server hinzufügen
builder.Services.AddGraphQLServer()
    .ModifyCostOptions(o => o.EnforceCostLimits = false)
    .RegisterDbContextFactory<SettingsServiceDbContext>()
    .AddMutationType<Mutation>()
    .AddTypeExtension<GraphQlMutationUserSetting>()
    .AddQueryType<Query>()
    .AddTypeExtension<GraphQlQueryUserSetting>()
    .AddType<UserSettingsType>()
    .AddDataLoader<UserSettingsDataLoader>()
    .AddFiltering()
    .AddSorting()
    .InitializeOnStartup();

//Add localization for ASP.NET Core
builder.Services.AddLocalization(options => options.ResourcesPath = "Localize");

//Register MediatR with the current assembly
if (!args.Contains("schema"))
{
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
}

//Setting the EndpointConventions for MassTransit
if (!args.Contains("schema"))
{
    Startup.ConfigureEndpointConventions(appSettings);
}

//Adding the consumers to the DI container
//builder.Services.AddScoped<UserAccountCreatedConsumer>();

//Adding MassTransit wit RabbitMQ
if (appSettings is not null && !args.Contains("schema"))
{
    builder.Services.AddMassTransit(x =>
    {
        //Hinzufügen der Consumer
        //x.AddConsumer<UserAccountCreatedConsumer>();

        //RabbitMq hinzufügen
        x.UsingRabbitMq((context, cfg) =>
        {
            var configuration = context.GetRequiredService<IConfiguration>();
            var host = configuration.GetConnectionString("RabbitMQConnection");
            cfg.Host(host);

            cfg.ReceiveEndpoint(appSettings.EndpointSettingsService, e =>
            {
                e.UseConcurrencyLimit(1);
                e.PrefetchCount = 16;
                e.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20)));

                //e.ConfigureConsumer<UserAccountCreatedConsumer>(context);
            });
        });
    });
}

try
{
    Log.Information("Building web app");

    var app = builder.Build();

    var lifetime = app.Lifetime;
    lifetime.ApplicationStarted.Register(() => Log.Information("Web app started"));
    lifetime.ApplicationStopped.Register(() => Log.Information("Application stopped"));
    
    var supportedCultures = new[]
    {
        new CultureInfo("de"),
        new CultureInfo("en")
    };

    Log.Information("Add logging to pipeline");
    app.UseSerilogRequestLogging();
    app.UseRequestLocalization(new RequestLocalizationOptions
    {
        DefaultRequestCulture = new RequestCulture("en-US"),
        SupportedCultures = supportedCultures,
        SupportedUICultures = supportedCultures
    });
    if (builder.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    Log.Information("Configure global exception handler");
    app.ConfigureExceptionHandler();

    if (!args.Contains("schema"))
    {
        Log.Information("Initialize and seed database");
        Startup.InitializeDatabase(app);
        Startup.SeedDatabase(app);
    }

    Log.Information("Add routing to pipeline");
    app.UseRouting();
    
    Log.Information("Add aspire endpoints to pipeline");
    app.MapDefaultEndpoints();
    
    Log.Information("Add GraphQl to pipeline");
    app.UseEndpoints(endpoints =>
    {
        if (builder.Environment.IsDevelopment())
        {
            endpoints.MapGraphQL();
        }
        else
        {
            endpoints.MapGraphQLHttp();
            endpoints.MapGraphQLWebSocket();
        }
    });

    Log.Information("Starting web app...");
    app.RunWithGraphQLCommands(args);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Web app terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
