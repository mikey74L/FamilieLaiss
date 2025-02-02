using Catalog.API;
using Catalog.API.GraphQL;
using Catalog.API.GraphQL.DataLoaders.Category;
using Catalog.API.GraphQL.DataLoaders.CategoryValue;
using Catalog.API.GraphQL.DataLoaders.Media;
using Catalog.API.GraphQL.DataLoaders.UploadPicture;
using Catalog.API.GraphQL.DataLoaders.UploadVideo;
using Catalog.API.GraphQL.Filter;
using Catalog.API.GraphQL.Mutations;
using Catalog.API.GraphQL.Mutations.Category;
using Catalog.API.GraphQL.Mutations.CategoryValue;
using Catalog.API.GraphQL.Mutations.MediaGroup;
using Catalog.API.GraphQL.Mutations.MediaItem;
using Catalog.API.GraphQL.Queries.Category;
using Catalog.API.GraphQL.Queries.CategoryValue;
using Catalog.API.GraphQL.Queries.Media;
using Catalog.API.GraphQL.Queries.UploadPicture;
using Catalog.API.GraphQL.Queries.UploadVideo;
using Catalog.API.GraphQL.Types.Category;
using Catalog.API.GraphQL.Types.CategoryValue;
using Catalog.API.GraphQL.Types.Media;
using Catalog.API.GraphQL.Types.UploadPicture;
using Catalog.API.GraphQL.Types.UploadVideo;
using Catalog.API.Logging;
using Catalog.API.MassTransit.Consumers.UploadPicture;
using Catalog.API.MassTransit.Consumers.UploadVideo;
using Catalog.API.Models;
using Catalog.Infrastructure.DBContext;
using GraphQL.Server.Ui.Voyager;
using InfrastructureHelper.EventDispatchHandler;
using MassTransit;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using ServiceLayerHelper;
using ServiceLayerHelper.Logging;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;
using System.Globalization;

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
builder.Services.AddHostedService<EventDispatcherBackgroundService>();

//Hinzuf�gen det globalen Exception-Handler Middleware
builder.Services.AddSingleton<ILog, LogSerilog>();

//Hinzuf�gen der Konfiguration (App-Settings) zum IOC-Container
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();

//Die DB-Context Factory hinzuf�gen inklusive der UnitOfWork
NpgsqlConnectionStringBuilder postgresConnectionStringBuilder = new()
{
    ApplicationName = "Catalog-Service",
    Host = appSettings?.PostgresHost,
    Port = appSettings?.PostgresPort ?? 0,
    Multiplexing = appSettings?.PostgresMultiplexing ?? false,
    Database = appSettings?.PostgresDatabase,
    Username = appSettings?.PostgresUser,
    Password = appSettings?.PostgresPassword
};
builder.Services.AddPooledDbContextFactory<CatalogServiceDbContext>(
        o => o.UseNpgsql(postgresConnectionStringBuilder.ToString()))
    .AddUnitOfWork<CatalogServiceDbContext>();

//Adding GraphQL Server
var graphQlBuilder = builder.Services.AddGraphQLServer()
    .RegisterDbContextFactory<CatalogServiceDbContext>()
    .AddMutationType<Mutation>()
    .AddTypeExtension<GraphQlMutationCategory>()
    .AddTypeExtension<GraphQlMutationCategoryValue>()
    .AddTypeExtension<GraphQlMutationMediaGroup>()
    .AddTypeExtension<GraphQlMutationMediaItem>()
    .AddQueryType<Query>()
    .AddTypeExtension<GraphQlQueryCategory>()
    .AddTypeExtension<GraphQlQueryCategoryValue>()
    .AddTypeExtension<GraphQlQueryMedia>()
    .AddTypeExtension<GraphQlQueryUploadPicture>()
    .AddTypeExtension<GraphQlQueryUploadVideo>()
    .AddType<GraphQlCategoryType>()
    .AddType<GraphQlCategoryValueType>()
    .AddType<GraphQlMediaGroupType>()
    .AddType<GraphQlMediaItemType>()
    .AddType<GraphQlMediaItemCategoryValueType>()
    .AddType<GraphQlUploadPictureType>()
    .AddType<GraphQlUploadVideoType>()
    .AddDataLoader<CategoryDataLoader>()
    .AddDataLoader<CategoryValueDataLoader>()
    .AddDataLoader<MediaGroupDataLoader>()
    .AddDataLoader<MediaItemDataLoader>()
    .AddDataLoader<UploadPictureDataLoader>()
    .AddDataLoader<UploadVideoDataLoader>()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .AddErrorFilter<GraphQlErrorFilter>()
    .AddAuthorization()
    .InitializeOnStartup();

//Lokalisierung f�r ASP.NET Core hinzuf�gen
builder.Services.AddLocalization(options => options.ResourcesPath = "Localize");

//Registrieren von MediatR mit der aktuellen Assembly
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

//Festlegen der EndpointConventions f�r MassTransit
Startup.ConfigureEndpointConventions(appSettings);

//Hinzuf�gen der Consumer zum DI-Container
builder.Services.AddScoped<UploadPictureCreatedConsumer>();
builder.Services.AddScoped<UploadVideoCreatedConsumer>();
builder.Services.AddScoped<UploadPictureDeletedConsumer>();
builder.Services.AddScoped<UploadVideoDeletedConsumer>();

if (appSettings is not null)
{
    builder.Services.AddMassTransit(x =>
    {
        x.AddConsumer<UploadPictureCreatedConsumer>();
        x.AddConsumer<UploadVideoCreatedConsumer>();
        x.AddConsumer<UploadPictureDeletedConsumer>();
        x.AddConsumer<UploadVideoDeletedConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(new Uri(appSettings.RabbitMqConnection));

            cfg.ReceiveEndpoint(appSettings.EndpointCatalogService, e =>
            {
                e.UseConcurrencyLimit(1);
                e.PrefetchCount = 16;
                e.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20)));

                e.ConfigureConsumer<UploadPictureCreatedConsumer>(context);
                e.ConfigureConsumer<UploadVideoCreatedConsumer>(context);
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

    //Initialisieren der Datenbank (Migration und Seeden)
    if (appSettings?.PostgresUser != "withoutdocker")
    {
        Startup.InitializeDatabase(app);
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