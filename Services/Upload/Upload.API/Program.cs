using Asp.Versioning;
using Hangfire;
using Hangfire.PostgreSql;
using InfrastructureHelper.EventDispatchHandler;
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
using Upload.API;
using Upload.API.Hangfire;
using Upload.API.Interfaces;
using Upload.API.Logging;
using Upload.API.MassTransit.Consumers;
using Upload.API.MicroServices;
using Upload.API.Models;
using Upload.API.Services;
using Upload.API.Swagger;
using Upload.DTO.FileUpload;
using Upload.Infrastructure.DBContext;

//Den Titel für das Konsolenfenster setzen
Console.Title = "Upload-Service";

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

//Hinzufügen der Service-Discovery
builder.AddServiceDiscovery(options => options.UseEureka());

//Hinzufügen eines HTTPContextAccessor 
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//Hinzufügen der Hosted-Services (Background-Services)
builder.Services.AddHostedService<EventDispatcherBackgroundService>();

//Hinzufügen det globalen Exception-Handler Middleware
builder.Services.AddSingleton<ILog, LogSerilog>();

//Add auto mapper with assembly registration
builder.Services.AddAutoMapper(typeof(Program));

//Alles für API-Versioning hinzufügen
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

//Alles für WebApi hinzufügen
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<SwaggerDefaultValues>();

    var fileNameMain = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
    var fileNameDto = typeof(AddUploadChunckDto).Assembly.GetName().Name + ".xml";
    var filePathMain = Path.Combine(AppContext.BaseDirectory, fileNameMain);
    var filePathDto = Path.Combine(AppContext.BaseDirectory, fileNameDto);

    options.IncludeXmlComments(filePathMain);
    options.IncludeXmlComments(filePathDto);
});

//Hinzufügen des Service für den Unique-Identifier
builder.Services.AddTransient<IUniqueIdentifierGenerator, UniqueIdentifierGeneratorService>();

//Hinzufügen der Services für die Microservice Zugriffe
builder.Services.AddSingleton<IGoogleMicroService, GoogleMicroService>();

//Hinzufügen der Konfiguration (App-Settings) zum IOC-Container
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
AppSettings? appSettings = appSettingsSection.Get<AppSettings>();

//Die DB-Context Factory hinzufügen inklusive der UnitOfWork
NpgsqlConnectionStringBuilder postgresConnectionStringBuilder = new();
postgresConnectionStringBuilder.ApplicationName = "Upload-Service";
postgresConnectionStringBuilder.Host = appSettings?.PostgresHost;
postgresConnectionStringBuilder.Port = appSettings?.PostgresPort ?? 0;
postgresConnectionStringBuilder.Multiplexing = appSettings?.PostgresMultiplexing ?? false;
postgresConnectionStringBuilder.Database = appSettings?.PostgresDatabase;
postgresConnectionStringBuilder.Username = appSettings?.PostgresUser;
postgresConnectionStringBuilder.Password = appSettings?.PostgresPassword;
builder.Services.AddPooledDbContextFactory<UploadServiceDBContext>(
        o => o.UseNpgsql(postgresConnectionStringBuilder.ToString()))
    .AddUnitOfWork<UploadServiceDBContext>();

//Hangfire konfigurieren
if (postgresConnectionStringBuilder is not null)
    _ = builder.Services.AddHangfire(options => options.UsePostgreSqlStorage(postgresConnectionStringBuilder.ConnectionString));
builder.Services.AddTransient<iJobOperations, JobOperationsService>();
builder.Services.AddTransient<JobExecuter>();
//Den Hangfire-Server zur Pipeline hinzufügen
//Dieser wird hier im Upload benötigt um eine Entkopplung zwischen dem Upload der Chuncks und dem Zusammenführen 
//der Chuncks zu einer Dateien zu realisieren.
//Beim löschen der Chuncks gab es im selben Aufruf immer eine Datei ist gesperrt. Durch Hangfire werden die Dateien
//erst später gelöscht und somit greift kein anderer Prozess mehr darauf zu
builder.Services.AddHangfireServer(x => x.ServerTimeout = TimeSpan.FromDays(1));

//Lokalisierung für ASP.NET Core hinzufügen
builder.Services.AddLocalization(options => options.ResourcesPath = "Localize");

//Registrieren von MediatR mit der aktuellen Assembly
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

//Festlegen der EndpointConventions für MassTransit
if (appSettings is not null)
{
    Startup.ConfigureEndpointConventions(appSettings);
}

//Hinzufügen der Consumer zum DI-Container
builder.Services.AddScoped<PictureInfoChangedConsumer>();
builder.Services.AddScoped<VideoInfoChangedConsumer>();
builder.Services.AddScoped<ExifDataChangedConsumer>();
builder.Services.AddScoped<PictureConvertedConsumer>();
builder.Services.AddScoped<UploadPictureAssignedConsumer>();
builder.Services.AddScoped<UploadVideoAssignedConsumer>();
builder.Services.AddScoped<MediaItemDeletedConsumer>();
builder.Services.AddScoped<VideoConvertedConsumer>();

//Mass-Transit konfigurieren
if (appSettings is not null)
{
    builder.Services.AddMassTransit(x =>
    {
        //Hinzufügen der Consumer
        x.AddConsumer<PictureInfoChangedConsumer>();
        x.AddConsumer<VideoInfoChangedConsumer>();
        x.AddConsumer<ExifDataChangedConsumer>();
        x.AddConsumer<PictureConvertedConsumer>();
        x.AddConsumer<UploadPictureAssignedConsumer>();
        x.AddConsumer<UploadVideoAssignedConsumer>();
        x.AddConsumer<MediaItemDeletedConsumer>();
        x.AddConsumer<VideoConvertedConsumer>();

        //RabbitMq hinzufügen
        x.UsingRabbitMq((context, cfg) =>
        {
            //Konfigurieren des Hosts
            cfg.Host(new Uri(appSettings.RabbitMQConnection));

            cfg.ReceiveEndpoint(appSettings.Endpoint_UploadService, e =>
            {
                e.UseConcurrencyLimit(1);
                e.PrefetchCount = 16;
                e.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20)));

                e.ConfigureConsumer<PictureInfoChangedConsumer>(context);
                e.ConfigureConsumer<VideoInfoChangedConsumer>(context);
                e.ConfigureConsumer<ExifDataChangedConsumer>(context);
                e.ConfigureConsumer<PictureConvertedConsumer>(context);
                e.ConfigureConsumer<UploadPictureAssignedConsumer>(context);
                e.ConfigureConsumer<UploadVideoAssignedConsumer>(context);
                e.ConfigureConsumer<MediaItemDeletedConsumer>(context);
                e.ConfigureConsumer<VideoConvertedConsumer>(context);
            });
        });
    });
}

//Den Web-Host ausführen
try
{
    Log.Information("Starting Web-Host...");

    var app = builder.Build();

    //Die von der Website unterstützen Sprachen hinzufügen
    var supportedCultures = new[]
    {
        new CultureInfo("de"),
        new CultureInfo("en")
    };

    //Hinzufügen des Request-Loggings von Serilog
    app.UseSerilogRequestLogging();

    //Lokalisierung anhand von Requests zur Pipeline hinzufügen
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

    //Swagger / OpenAPI hinzufügen
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

    //Controller hinzufügen
    app.MapControllers();

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
