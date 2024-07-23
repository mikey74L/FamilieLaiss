using Asp.Versioning;
using Google.API;
using Google.API.GraphQl.Queries;
using Google.API.Interfaces;
using Google.API.Logging;
using Google.API.Models;
using Google.API.Services;
using Google.API.Swagger;
using Google.DTO;
using MassTransit;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Serilog;
using ServiceLayerHelper.Logging;
using StackExchange.Redis;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;
using System.Reflection;

// Set the title for the console window
Console.Title = "Google-Service";

var builder = WebApplication.CreateBuilder(args);

// Logging
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Host.UseSerilog(logger);

// Create the logger for Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateBootstrapLogger();

// Add service discovery
builder.AddServiceDiscovery(options => options.UseEureka());

// Add an HttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add global exception handler middleware
builder.Services.AddSingleton<ILog, LogSerilog>();

// Add everything for API versioning
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

// Add everything for WebApi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<SwaggerDefaultValues>();

    var fileNameMain = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
    var fileNameDTO = typeof(GoogleGeoCodingAdressDTO).Assembly.GetName().Name + ".xml";
    var filePathMain = System.IO.Path.Combine(AppContext.BaseDirectory, fileNameMain);
    var filePathDTO = System.IO.Path.Combine(AppContext.BaseDirectory, fileNameDTO);

    options.IncludeXmlComments(filePathMain);
    options.IncludeXmlComments(filePathDTO);
});

// Add the configuration (App-Settings) to the IOC container
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
AppSettings? appSettings = appSettingsSection.Get<AppSettings>();

// Add localization for ASP.NET Core
builder.Services.AddLocalization(options => options.ResourcesPath = "Localize");

// Register MediatR with the current assembly
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

// Register the GoogleGeoCoding service
builder.Services.AddTransient<IWsGoogleGeoCoding, WsGoogleGeoCodingService>();

//Redis Multiplexer hinzuf�gen wird f�r GraphQL Schema Stitching verwendet
builder.Services.AddSingleton(ConnectionMultiplexer.Connect("redis"));

//Adding GraphQL Server
var graphQlBuilder = builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddTypeExtension<GraphQlQueryGoogle>()
    .AddAuthorization()
    .InitializeOnStartup()
    .PublishSchemaDefinition(c => c
        // The name of the schema. This name should be unique
        .SetName("google")
        .PublishToRedis(
            // The configuration name under which the schema should be published
            "familielaiss",
            // The connection multiplexer that should be used for publishing
            sp => sp.GetRequiredService<ConnectionMultiplexer>()
        )
    );

// Set the EndpointConventions for MassTransit
Startup.ConfigureEndpointConventions(appSettings);

// Add the Consumer to the DI container
//builder.Services.AddScoped<PictureInfoChangedConsumer>();

// Configure Mass-Transit
if (appSettings is not null)
{
    builder.Services.AddMassTransit(x =>
    {
        // Add the Consumer
        //x.AddConsumer<PictureUploadedConsumer>();

        // Add RabbitMq
        x.UsingRabbitMq((context, cfg) =>
        {
            // Configure the Host
            cfg.Host(new Uri(appSettings.RabbitMqConnection));

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

// Run the Web-Host
try
{
    Log.Information("Starting Web-Host...");

    var app = builder.Build();

    // Add the supported languages for the website
    var supportedCultures = new[]
    {
        new CultureInfo("de"),
        new CultureInfo("en")
    };

    // Add Serilog Request Logging
    app.UseSerilogRequestLogging();

    // Add localization based on requests to the pipeline
    app.UseRequestLocalization(new RequestLocalizationOptions
    {
        DefaultRequestCulture = new RequestCulture("en-US"),
        SupportedCultures = supportedCultures,
        SupportedUICultures = supportedCultures
    });

    // If in development mode, show a detailed exception page
    if (builder.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    // Configure the Exception Handler Middleware
    app.ConfigureExceptionHandler();

    // Add Swagger / OpenAPI
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var descriptions = app.DescribeApiVersions();

            // Build a swagger endpoint for each discovered API version
            foreach (var description in descriptions)
            {
                var url = $"/swagger/{description.GroupName}/swagger.json";
                var name = description.GroupName.ToUpperInvariant();
                options.SwaggerEndpoint(url, name);
            }
        });
    }

    //Add routing to pipeline
    app.UseRouting();

    //Initialize endpoints for GraphQL
    if (builder.Environment.IsDevelopment())
    {
        app.MapGraphQL();
    }
    else
    {
        app.MapGraphQLHttp();
        app.MapGraphQLWebSocket();
    }

    // Add Controllers
    app.MapControllers();

    //Start the API
    app.RunWithGraphQLCommands(args);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Web-Host terminated unexpectedly");
}
finally
{
    Log.Information("Web-Host stopped");
    Log.CloseAndFlush();
}