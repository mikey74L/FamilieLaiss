using Asp.Versioning;
using Catalog.API;
using Catalog.API.BackgroundServices;
using Catalog.API.Extensions;
using Catalog.API.Logging;
using Catalog.API.MassTransit.Consumers;
using Catalog.API.Models;
using Catalog.API.Swagger;
using Catalog.DTO.Category;
using Catalog.Infrastructure.DBContext;
using InfrastructureHelper.EventDispatchHandler;
using Mapster;
using MassTransit;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using Serilog;
using ServiceLayerHelper;
using ServiceLayerHelper.Logging;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;
using System.Reflection;

Console.Title = "Catalog-Service";

var builder = WebApplication.CreateBuilder(args);

//Logging
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Host.UseSerilog(logger);

//Den Logger f�r Serilog erstellen
Log.Logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .CreateBootstrapLogger();

//Hinzuf�gen der Service-Discovery
builder.AddServiceDiscovery(options => options.UseEureka());

//Hinzuf�gen eines HTTPContextAccessor 
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//Hinzuf�gen der Hosted-Services (Background-Services)
builder.Services.AddHostedService<MessageForAddedMediaItemsBackgroundService>();
builder.Services.AddHostedService<EventDispatcherBackgroundService>();

//Hinzuf�gen det globalen Exception-Handler Middleware
builder.Services.AddSingleton<ILog, LogSerilog>();

//Mapster konfigurieren
builder.Services.AddMapster();
builder.Services.AddMapsterTypeConfigurations();

//Alles f�r API-Versioning hinzuf�gen
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

//Alles f�r WebApi hinzuf�gen
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<SwaggerDefaultValues>();

    var fileNameMain = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
    var fileNameDTO = typeof(CategoryDTO).Assembly.GetName().Name + ".xml";
    var filePathMain = Path.Combine(AppContext.BaseDirectory, fileNameMain);
    var filePathDTO = Path.Combine(AppContext.BaseDirectory, fileNameDTO);

    options.IncludeXmlComments(filePathMain);
    options.IncludeXmlComments(filePathDTO);
});

//Hinzuf�gen der Konfiguration (App-Settings) zum IOC-Container
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
AppSettings? appSettings = appSettingsSection.Get<AppSettings>();

//Die DB-Context Factory hinzuf�gen inklusive der UnitOfWork
NpgsqlConnectionStringBuilder postgresConnectionStringBuilder = new();
postgresConnectionStringBuilder.ApplicationName = "Catalog-Service";
postgresConnectionStringBuilder.Host = appSettings?.PostgresHost;
postgresConnectionStringBuilder.Port = appSettings?.PostgresPort ?? 0;
postgresConnectionStringBuilder.Multiplexing = appSettings?.PostgresMultiplexing ?? false;
postgresConnectionStringBuilder.Database = appSettings?.PostgresDatabase;
postgresConnectionStringBuilder.Username = appSettings?.PostgresUser;
postgresConnectionStringBuilder.Password = appSettings?.PostgresPassword;
builder.Services.AddPooledDbContextFactory<CatalogServiceDbContext>(
    o => o.UseNpgsql(postgresConnectionStringBuilder.ToString()))
.AddUnitOfWork<CatalogServiceDbContext>();

//Lokalisierung f�r ASP.NET Core hinzuf�gen
builder.Services.AddLocalization(options => options.ResourcesPath = "Localize");

//Registrieren von MediatR mit der aktuellen Assembly
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

//Festlegen der EndpointConventions f�r MassTransit
Startup.ConfigureEndpointConventions(appSettings);

//Hinzuf�gen der Consumer zum DI-Container
builder.Services.AddScoped<PictureUploadedConsumer>();
builder.Services.AddScoped<VideoUploadedConsumer>();
builder.Services.AddScoped<UploadPictureDeletedConsumer>();
builder.Services.AddScoped<UploadVideoDeletedConsumer>();

if (appSettings is not null)
{
    builder.Services.AddMassTransit(x =>
    {
        //Hinzuf�gen der Consumer
        x.AddConsumer<PictureUploadedConsumer>();
        x.AddConsumer<VideoUploadedConsumer>();
        x.AddConsumer<UploadPictureDeletedConsumer>();
        x.AddConsumer<UploadVideoDeletedConsumer>();

        //RabbitMq hinzuf�gen
        x.UsingRabbitMq((context, cfg) =>
        {
            //Konfigurieren des Hosts
            cfg.Host(new Uri(appSettings.RabbitMQConnection));

            cfg.ReceiveEndpoint(appSettings.Endpoint_CatalogService, e =>
            {
                e.UseConcurrencyLimit(1);
                e.PrefetchCount = 16;
                e.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20)));

                e.ConfigureConsumer<PictureUploadedConsumer>(context);
                e.ConfigureConsumer<VideoUploadedConsumer>(context);
                e.ConfigureConsumer<UploadPictureDeletedConsumer>(context);
                e.ConfigureConsumer<UploadVideoDeletedConsumer>(context);
            });
        });
    });
}

//Den Web-Host ausf�hren
try
{
    Log.Information("Starting Web-Host...");

    var app = builder.Build();

    //Die von der Website unterst�tzen Sprachen hinzuf�gen
    var supportedCultures = new[]
    {
        new CultureInfo("de"),
        new CultureInfo("en")
    };

    //Hinzuf�gen des Request-Loggings von Serilog
    app.UseSerilogRequestLogging();

    //Lokalisierung anhand von Requests zur Pipeline hinzuf�gen
    app.UseRequestLocalization(new RequestLocalizationOptions
    {
        DefaultRequestCulture = new RequestCulture("en-US"),
        SupportedCultures = supportedCultures,
        SupportedUICultures = supportedCultures
    });

    //Wenn im Entwicklungsmodus dann wird eine detaillierte Exception-Page angezeigt
    if (builder.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    //Konfigurieren der Exception-Handler-Middleware
    app.ConfigureExceptionHandler();

    //Swagger / OpenAPI hinzuf�gen
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

    //Initialisieren der Datenbank (Migration und Seeden)
    Startup.InitializeDatabase(app);

    //Controller hinzuf�gen
    app.MapControllers();

    //Starten der API
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
