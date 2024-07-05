using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Picture.API.Logging;
using Picture.API.Models;
using Serilog;
using ServiceLayerHelper.Logging;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;

//Set Title for Console-Window
Console.Title = "Picture-Service";

//Create web application builder
var builder = WebApplication.CreateBuilder(args);

//Logging
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Host.UseSerilog(logger);

//Create logger for serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateBootstrapLogger();

//Adding Service-Discovery
builder.Host.AddServiceDiscovery(options => options.UseEureka());

//Adding HTTPContextAccessor 
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//Adding logger service for global exception handler
builder.Services.AddSingleton<ILog, LogSerilog>();

//Adding controllers
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.MaxDepth = 10;
});

//Adding CORS
builder.Services.AddCors(p =>
    p.AddPolicy("PictureApi",
        corsPolicyBuilder => { corsPolicyBuilder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader(); }));

//Adding config (App-Settings) 
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);

//Configuring and executing web host
try
{
    Log.Information("Starting Web-Host...");

    var app = builder.Build();

    //Adding supported languages
    var supportedCultures = new[]
    {
        new CultureInfo("de"),
        new CultureInfo("en")
    };

    //Adding request logging for serilog
    app.UseSerilogRequestLogging();

    //Localize per request
    app.UseRequestLocalization(new RequestLocalizationOptions
    {
        DefaultRequestCulture = new RequestCulture("en-US"),
        SupportedCultures = supportedCultures,
        SupportedUICultures = supportedCultures
    });

    //Adding developer exception page when in dev environment
    if (builder.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    //Configure Exception-Handler-Middleware
    app.ConfigureExceptionHandler();

    //Add CORS
    app.UseCors("PictureApi");

    //Add routing
    app.UseRouting();

    //Add authentication when not in development environment
    if (!builder.Environment.IsDevelopment())
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }

    //Add controllers
    app.MapControllers();

    app.Run();
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