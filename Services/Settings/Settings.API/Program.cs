using GraphQL.Server.Ui.Voyager;
using InfrastructureHelper.EventDispatchHandler;
using MassTransit;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using ServiceLayerHelper;
using ServiceLayerHelper.Logging;
using Settings.API;
using Settings.API.GraphQL.Mutations;
using Settings.API.GraphQL.Mutations.UserSettings;
using Settings.API.GraphQL.Queries;
using Settings.API.GraphQL.Types;
using Settings.API.Logging;
using Settings.API.Models;
using Settings.Infrastructure.DBContext;
using StackExchange.Redis;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;
using System.Globalization;
using User.API.GraphQL.Queries.UserSettings;

//Den Titel für das Konsolenfenster setzen
Console.Title = "Settings-Service";

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

//Hinzufügen der Konfiguration (App-Settings) zum IOC-Container
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
AppSettings? appSettings = appSettingsSection.Get<AppSettings>();

//Die DB-Context Factory hinzufügen inklusive der UnitOfWork
NpgsqlConnectionStringBuilder postgresConnectionStringBuilder = new()
{
    ApplicationName = "Settings-Service",
    Host = appSettings?.PostgresHost,
    Port = appSettings?.PostgresPort ?? 0,
    Multiplexing = appSettings?.PostgresMultiplexing ?? false,
    Database = appSettings?.PostgresDatabase,
    Username = appSettings?.PostgresUser,
    Password = appSettings?.PostgresPassword
};
builder.Services.AddPooledDbContextFactory<SettingsServiceDbContext>(
        o => o.UseNpgsql(postgresConnectionStringBuilder.ToString()))
    .AddUnitOfWork<SettingsServiceDbContext>();

//Redis Multiplexer hinzufügen wird für GraphQL Schema Stitching verwendet
builder.Services.AddSingleton(ConnectionMultiplexer.Connect("redis"));

//Den GraphQL-Server hinzufügen
var GraphQLBuilder = builder.Services.AddGraphQLServer()
    .RegisterDbContext<SettingsServiceDbContext>(DbContextKind.Pooled)
    .AddMutationType<Mutation>()
    .AddTypeExtension<GraphQlMutationUserSetting>()
    .AddQueryType<Query>()
    .AddTypeExtension<GraphQlQueryUserSetting>()
    .AddType<UserSettingsType>()
    .AddFiltering()
    .AddSorting()
    .InitializeOnStartup() //Schema beim Startup initialisieren und nicht beim ersten Request wegen Publish mit Redis
    .PublishSchemaDefinition(c => c
        // The name of the schema. This name should be unique
        .SetName("usersetting")
        .PublishToRedis(
            // The configuration name under which the schema should be published
            "familielaiss",
            // The connection multiplexer that should be used for publishing
            sp => sp.GetRequiredService<ConnectionMultiplexer>()
        )
    );
if (!builder.Environment.IsDevelopment())
{
    GraphQLBuilder.AddAuthorization();
}

//Lokalisierung für ASP.NET Core hinzufügen
builder.Services.AddLocalization(options => options.ResourcesPath = "Localize");

//Registrieren von MediatR mit der aktuellen Assembly
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

//Festlegen der EndpointConventions für MassTransit
Startup.ConfigureEndpointConventions(appSettings);

//Hinzufügen der Consumer zum DI-Container
//builder.Services.AddScoped<UserAccountCreatedConsumer>();

//Mass-Transit konfigurieren
if (appSettings is not null)
{
    builder.Services.AddMassTransit(x =>
    {
        //Hinzufügen der Consumer
        //x.AddConsumer<UserAccountCreatedConsumer>();

        //RabbitMq hinzufügen
        x.UsingRabbitMq((context, cfg) =>
        {
            //Konfigurieren des Hosts
            cfg.Host(new Uri(appSettings.RabbitMqConnection));

            cfg.ReceiveEndpoint(appSettings.EndpointSettingsService, e =>
            {
                e.UseConcurrencyLimit(1);
                e.PrefetchCount = 16;
                e.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20)));

                //e.ConfigureConsumer<UserAccountCreatedConsumer>(context);
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

    //Routing hinzufügen
    app.UseRouting();

    //Initialisieren der Datenbank (Migration und Seeden)
    Startup.InitializeDatabase(app);
    Startup.SeedDatabase(app);

    //Authentifizierung verwenden
    if (!builder.Environment.IsDevelopment())
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }

    //Initialisieren der Endpoints für GraphQL
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

    //Initialisieren des Endpoints für GraphQL-Voyager
    if (builder.Environment.IsDevelopment())
    {
        app.UseGraphQLVoyager("/graphql-voyager", new VoyagerOptions()
        {
            GraphQLEndPoint = "/graphql"
        });
    }

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
