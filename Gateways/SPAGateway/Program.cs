using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Serilog;
using ServiceLayerHelper.Logging;
using SPAGateway.GraphQl;
using SPAGateway.Logging;
using SPAGateway.Models;
using StackExchange.Redis;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;

//Den Titel für das Konsolenfenster setzen
Console.Title = "Gateway-SPA";

var builder = WebApplication.CreateBuilder(args);

//Hinzufügen der Ocelot-Konfiguration über die JSON-Files
builder.Configuration.AddOcelot(System.IO.Path.Combine(builder.Environment.ContentRootPath, "Configuration"),
    builder.Environment);

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

//Hinzufügen der Service-Discovery
builder.AddServiceDiscovery(options => options.UseEureka());

//Hinzufügen det globalen Exception-Handler Middleware
builder.Services.AddSingleton<ILog, LogSerilog>();

//Mapster hinzufügen
//builder.Services.AddMapster();

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
            policy.WithOrigins(appSettings.CORS_Origin)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    });
}

//Add Http-Clients for all GraphQL Micro-Services
builder.Services
    .AddHttpClient(WellKnownSchemaNames.Catalog, c => c.BaseAddress = new Uri("http://catalogservice/graphql"))
    .AddRoundRobinLoadBalancer();

//Add redis for GraphQL-Schema-Stitching
builder.Services.AddSingleton(ConnectionMultiplexer.Connect("redis"));

//Add GraphQL-Server
//Add GraphQL-Server
builder.Services.AddGraphQLServer()
    .AddQueryType(d => d.Name("Query"))
    .AddRemoteSchema(WellKnownSchemaNames.Catalog);
//.AddRemoteSchemasFromRedis("familielaiss", sp => sp.GetRequiredService<ConnectionMultiplexer>())
//.AddTypeExtensionsFromFile("./Stitching.graphql");

//Add Ocelot to DI-Container
//builder.Services.AddOcelot(builder.Configuration).AddEureka();

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

    //Ocelot zur Pipeline hinzufügen
    //app.UseOcelot().Wait();

    app.Run();
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