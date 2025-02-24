using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using ServiceLayerHelper.Logging;
using SPAGateway.Logging;
using SPAGateway.Models;
using System;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

//Den Titel für das Konsolenfenster setzen
Console.Title = "Gateway-SPA";

var builder = WebApplication.CreateBuilder(args);

//Integrate Aspire
builder
    .AddServiceDefaults();

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

//Adding the global exception handler middleware
builder.Services.AddSingleton<ILog, LogSerilog>();

//Adding configuration (App-Settings) to the IOC container
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
AppSettings? appSettings = appSettingsSection.Get<AppSettings>();

//Cors konfigurieren und zum DI-Container hinzufügen
builder.Services.AddCors();

//Add header propagation
builder.Services.AddHeaderPropagation(
    options =>
    {
        options.Headers.Add("GraphQL-Preflight");
        options.Headers.Add("Authorization");
    });

//Add HttpClient for Fusion
builder.Services
    .AddHttpClient("Fusion")
    .AddHeaderPropagation();

//Add GraphQL-Server
builder.Services.AddFusionGatewayServer()
    .ConfigureFromFile("gateway.fgp")
    .ModifyFusionOptions(x => x.AllowQueryPlan = true)
    .AddServiceDiscoveryRewriter();

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

    Log.Information("Add web sockets to pipeline");
    app.UseWebSockets();

    Log.Information("Add Cors to pipeline");
    app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

    Log.Information("Add Header Propagation to pipeline");
    app.UseHeaderPropagation();
    
    Log.Information("Add routing to pipeline");
    app.UseRouting();

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
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Web app terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}