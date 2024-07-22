using GraphQL.Server.Ui.Voyager;
using Hangfire;
using Hangfire.PostgreSql;
using InfrastructureHelper.EventDispatchHandler;
using MassTransit;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using ServiceLayerHelper;
using ServiceLayerHelper.Logging;
using StackExchange.Redis;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;
using System.Globalization;
using Upload.API;
using Upload.API.GraphQL.DataLoader.UploadPicture;
using Upload.API.GraphQL.DataLoader.UploadVideo;
using Upload.API.GraphQL.Filter;
using Upload.API.GraphQL.Mutations;
using Upload.API.GraphQL.Mutations.FileUpload;
using Upload.API.GraphQL.Mutations.UploadPicture;
using Upload.API.GraphQL.Mutations.UploadVideo;
using Upload.API.GraphQL.Queries;
using Upload.API.GraphQL.Queries.FileUpload;
using Upload.API.GraphQL.Queries.UploadPicture;
using Upload.API.GraphQL.Queries.UploadVideo;
using Upload.API.GraphQL.Types.UploadPicture;
using Upload.API.GraphQL.Types.UploadVideo;
using Upload.API.Hangfire;
using Upload.API.Interfaces;
using Upload.API.Logging;
using Upload.API.MassTransit.Consumers.MediaItem;
using Upload.API.MassTransit.Consumers.UploadPicture;
using Upload.API.MicroServices;
using Upload.API.Models;
using Upload.API.Services;
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

//Adding unique identifier service
builder.Services.AddTransient<IUniqueIdentifierGenerator, UniqueIdentifierGeneratorService>();

//Adding Google MicroService
builder.Services.AddSingleton<IGoogleMicroService, GoogleMicroService>();

//Hinzufügen det globalen Exception-Handler Middleware
builder.Services.AddSingleton<ILog, LogSerilog>();

//Hinzufügen der Konfiguration (App-Settings) zum IOC-Container
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
AppSettings? appSettings = appSettingsSection.Get<AppSettings>();

//Die DB-Context Factory hinzufügen inklusive der UnitOfWork
NpgsqlConnectionStringBuilder postgresConnectionStringBuilder = new()
{
    ApplicationName = "Upload-Service",
    Host = appSettings?.PostgresHost,
    Port = appSettings?.PostgresPort ?? 0,
    Multiplexing = appSettings?.PostgresMultiplexing ?? false,
    Database = appSettings?.PostgresDatabase,
    Username = appSettings?.PostgresUser,
    Password = appSettings?.PostgresPassword
};
builder.Services.AddPooledDbContextFactory<UploadServiceDbContext>(
        o => o.UseNpgsql(postgresConnectionStringBuilder.ToString()))
    .AddUnitOfWork<UploadServiceDbContext>();

//Add hangfire 
builder.Services.AddHangfire(options =>
    options.UsePostgreSqlStorage(o => { o.UseNpgsqlConnection(postgresConnectionStringBuilder.ConnectionString); }));
builder.Services.AddTransient<IJobOperations, JobOperationsService>();
builder.Services.AddTransient<JobExecutor>();
builder.Services.AddHangfireServer(x => x.ServerTimeout = TimeSpan.FromDays(1));

//Redis Multiplexer hinzufügen wird für GraphQL Schema Stitching verwendet
builder.Services.AddSingleton(ConnectionMultiplexer.Connect("redis"));

//Adding GraphQL Server
var graphQlBuilder = builder.Services.AddGraphQLServer()
    .RegisterDbContext<UploadServiceDbContext>(DbContextKind.Pooled)
    //.AddDiagnosticEventListener<QueryLogger>()
    .AddMutationType<Mutation>()
    .AddTypeExtension<GraphQlMutationUploadPicture>()
    .AddTypeExtension<GraphQlMutationUploadVideo>()
    .AddTypeExtension<GraphQlMutationFileUpload>()
    .AddQueryType<Query>()
    .AddTypeExtension<GraphQlQueryUploadPicture>()
    .AddTypeExtension<GraphQlQueryUploadVideo>()
    .AddTypeExtension<GraphQlQueryFileUpload>()
    .AddType<GraphQlUploadPictureType>()
    .AddType<GraphQlUploadVideoType>()
    .AddDataLoader<UploadPictureDataLoader>()
    .AddDataLoader<UploadVideoDataLoader>()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .AddErrorFilter<GraphQlErrorFilter>()
    .InitializeOnStartup()
    .PublishSchemaDefinition(c => c
        // The name of the schema. This name should be unique
        .SetName("upload")
        .PublishToRedis(
            // The configuration name under which the schema should be published
            "familielaiss",
            // The connection multiplexer that should be used for publishing
            sp => sp.GetRequiredService<ConnectionMultiplexer>()
        )
    );


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
builder.Services.AddScoped<MediaItemCreatedConsumer>();
builder.Services.AddScoped<MediaItemDeletedConsumer>();
builder.Services.AddScoped<SetUploadPictureExifInfoConsumer>();
builder.Services.AddScoped<SetUploadPictureDimensionsConsumer>();

//Mass-Transit konfigurieren
if (appSettings is not null)
{
    builder.Services.AddMassTransit(x =>
    {
        //Hinzufügen der Consumer
        x.AddConsumer<MediaItemCreatedConsumer>();
        x.AddConsumer<MediaItemDeletedConsumer>();
        x.AddConsumer<SetUploadPictureExifInfoConsumer>();
        x.AddConsumer<SetUploadPictureDimensionsConsumer>();

        //RabbitMq hinzufügen
        x.UsingRabbitMq((context, cfg) =>
        {
            //Konfigurieren des Hosts
            cfg.Host(new Uri(appSettings.RabbitMqConnection));

            cfg.ReceiveEndpoint(appSettings.EndpointUploadService, e =>
            {
                e.UseConcurrencyLimit(1);
                e.PrefetchCount = 16;
                e.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20)));

                e.ConfigureConsumer<MediaItemCreatedConsumer>(context);
                e.ConfigureConsumer<MediaItemDeletedConsumer>(context);
                e.ConfigureConsumer<SetUploadPictureExifInfoConsumer>(context);
                e.ConfigureConsumer<SetUploadPictureDimensionsConsumer>(context);
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

    //Initialisieren der Datenbank (Migration und Seeden)
    Startup.InitializeDatabase(app);

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

    //Initialize Endpoints for GraphQL-Voyager
    if (builder.Environment.IsDevelopment())
    {
        app.UseGraphQLVoyager("/graphql-voyager", new VoyagerOptions()
        {
            GraphQLEndPoint = "/graphql"
        });
    }

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