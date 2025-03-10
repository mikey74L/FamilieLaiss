using Asp.Versioning;
using Google.API;
using Google.API.GraphQl.Queries;
using Google.API.Interfaces;
using Google.API.Logging;
using Google.API.Models;
using Google.API.Services;
using MassTransit;
using Microsoft.AspNetCore.Localization;
using Serilog;
using ServiceLayerHelper.Logging;
using System.Globalization;

Console.Title = "Google-Service";

var builder = WebApplication.CreateBuilder(args);

//Integrate Aspire
builder
    .AddServiceDefaults()
    .AddAzureKeyVaultClient("key-vault");

if (!args.Contains("schema"))
{
    builder
        .Configuration.AddAzureKeyVaultSecrets("key-vault");
}

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

//Adding the global exception handler middleware
builder.Services.AddSingleton<ILog, LogSerilog>();

//Add everything for API versioning
var apiVersioningBuilder = builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.ReportApiVersions = true;
});
apiVersioningBuilder.AddApiExplorer(
    options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

//Add everything for WebApi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//Add the configuration (App-Settings) to the IOC container
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
AppSettings? appSettings = appSettingsSection.Get<AppSettings>();

//Add localization for ASP.NET Core
builder.Services.AddLocalization(options => options.ResourcesPath = "Localize");

//Register MediatR with the current assembly
if (!args.Contains("schema"))
{
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
}

//Register the GoogleGeoCoding service
builder.Services.AddTransient<IWsGoogleGeoCoding, WsGoogleGeoCodingService>();

//Adding GraphQL Server
builder.Services.AddGraphQLServer()
    .ModifyCostOptions(o => o.EnforceCostLimits = false)
    .AddQueryType<Query>()
    .AddTypeExtension<GraphQlQueryGoogle>()
    .AddAuthorization()
    .InitializeOnStartup();

//Set the EndpointConventions for MassTransit
if (!args.Contains("schema"))
{
    Startup.ConfigureEndpointConventions(appSettings);
}

//Add the Consumer to the DI container
//builder.Services.AddScoped<PictureInfoChangedConsumer>();

// Configure Mass-Transit
if (appSettings is not null && !args.Contains("schema"))
{
    builder.Services.AddMassTransit(x =>
    {
        // Add the Consumer
        //x.AddConsumer<PictureUploadedConsumer>();

        // Add RabbitMq
        x.UsingRabbitMq((context, cfg) =>
        {
            var configuration = context.GetRequiredService<IConfiguration>();
            var host = configuration.GetConnectionString("RabbitMQConnection");
            cfg.Host(host);

            cfg.ReceiveEndpoint(appSettings.EndpointGoogleApiService, e =>
            {
                e.UseConcurrencyLimit(1);
                e.PrefetchCount = 16;
                e.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20)));

                //e.ConfigureConsumer<PictureUploadedConsumer>(context);
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

    Log.Information("Add routing to pipeline");
    app.UseRouting();

    Log.Information("Add aspire endpoints to pipeline");
    app.MapDefaultEndpoints();

    Log.Information("Add GraphQl to pipeline");
    if (builder.Environment.IsDevelopment())
    {
        app.MapGraphQL();
    }
    else
    {
        app.MapGraphQLHttp();
        app.MapGraphQLWebSocket();
    }

    Log.Information("Add controllers to pipeline");
    app.MapControllers();

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