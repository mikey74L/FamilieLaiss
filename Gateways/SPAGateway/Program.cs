using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using ServiceLayerHelper.Logging;
using SPAGateway.GraphQl;
using SPAGateway.Logging;
using SPAGateway.Models;
using StackExchange.Redis;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;
using System;

//Den Titel für das Konsolenfenster setzen
Console.Title = "Gateway-SPA";

var builder = WebApplication.CreateBuilder(args);

//Logging
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Host.UseSerilog(logger);

//Den Logger für Serilog erstellen
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateBootstrapLogger();

Log.Information("TestLogging");
//Hinzufügen der Service-Discovery
builder.AddServiceDiscovery(options => options.UseEureka());

//Hinzufügen det globalen Exception-Handler Middleware
builder.Services.AddSingleton<ILog, LogSerilog>();

//Hinzufügen der Konfiguration (App-Settings) zum IOC-Container
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
AppSettings appSettings = appSettingsSection.Get<AppSettings>();

//Cors konfigurieren und zum DI-Container hinzufügen
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("SPAGateway", policy =>
        {
            policy.WithOrigins(appSettings.CorsOrigin)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    });
}

//Add Http-Clients for all GraphQL Micro-Services
builder.Services
    .AddHttpClient(WellKnownSchemaNames.UserSetting, c => c.BaseAddress = new Uri("http://settingsservice/graphql"))
    .AddRoundRobinLoadBalancer();
builder.Services
    .AddHttpClient(WellKnownSchemaNames.Catalog, c => c.BaseAddress = new Uri("http://catalogservice/graphql"))
    .AddRoundRobinLoadBalancer();
builder.Services
    .AddHttpClient(WellKnownSchemaNames.Upload, c => c.BaseAddress = new Uri("http://uploadservice/graphql"))
    .AddRoundRobinLoadBalancer();
builder.Services
    .AddHttpClient(WellKnownSchemaNames.PictureConvert,
        c => c.BaseAddress = new Uri("http://pictureconvertservice/graphql"))
    .AddRoundRobinLoadBalancer();
builder.Services
    .AddHttpClient(WellKnownSchemaNames.VideoConvert,
        c => c.BaseAddress = new Uri("http://videoconvertservice/graphql"))
    .AddRoundRobinLoadBalancer();

//Add redis for GraphQL-Schema-Stitching
builder.Services.AddSingleton(ConnectionMultiplexer.Connect("redis"));

//Add GraphQL-Server
builder.Services.AddGraphQLServer()
    .AddRemoteSchemasFromRedis("familielaiss", sp => sp.GetRequiredService<ConnectionMultiplexer>())
    .IgnoreField("Query", "uploadPicture", WellKnownSchemaNames.Catalog)
    .IgnoreField("Query", "uploadVideo", WellKnownSchemaNames.Catalog)
    .AddTypeExtensionsFromFile("./Stitching.graphql");

//Den Web-Host ausführen
try
{
    Log.Information("Starting Web-Host...");

    var app = builder.Build();

    //Hinzufügen des Request-Loggings von Serilog
    app.UseSerilogRequestLogging();

    //Wenn im Entwicklungsmodus dann wird eine detaillierte Exception-Page angezeigt
    if (builder.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    //Cors zur Pipeline hinzufügen
    if (builder.Environment.IsDevelopment()) app.UseCors("SPAGateway");

    //Konfigurieren der Exception-Handler-Middleware
    app.ConfigureExceptionHandler();

    //WebSockets zur Pipeline hinzufügen
    app.UseWebSockets();

    //Routing hinzufügen
    app.UseRouting();

    //Initialisieren der Endpoints für GraphQL
    app.MapGraphQL();

    app.RunWithGraphQLCommands(args);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Web-Host terminated unexpectedly");
}
finally
{
    Log.Information("Web-Host stoped");
    Log.CloseAndFlush();
}