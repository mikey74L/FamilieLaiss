using GraphQL.Server.Ui.Voyager;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using Serilog;
using ServiceLayerHelper;
using ServiceLayerHelper.Logging;
using StackExchange.Redis;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;
using System;
using System.Globalization;
using User.API;
using User.API.GraphQL.Mutations;
using User.API.GraphQL.Mutations.UserMutations;
using User.API.GraphQL.Queries;
using User.API.GraphQL.Queries.Country;
using User.API.GraphQL.Queries.UserQuery;
using User.API.GraphQL.Types;
using User.API.Logging;
using User.API.Models;
using User.Infrastructure.DBContext;


//Den Titel f�r das Konsolenfenster setzen
Console.Title = "User-Service";

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
//builder.Services.AddHostedService<MessageForAddedMediaItemsBackgroundService>();

//Hinzuf�gen det globalen Exception-Handler Middleware
builder.Services.AddSingleton<ILog, LogSerilog>();

//Hinzuf�gen der Konfiguration (App-Settings) zum IOC-Container
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
AppSettings? appSettings = appSettingsSection.Get<AppSettings>();

//Die DB-Context Factory hinzuf�gen inklusive der UnitOfWork
NpgsqlConnectionStringBuilder postgresConnectionStringBuilder = new();
postgresConnectionStringBuilder.ApplicationName = "User-Service";
postgresConnectionStringBuilder.Host = appSettings?.PostgresHost;
postgresConnectionStringBuilder.Port = appSettings?.PostgresPort ?? 0;
postgresConnectionStringBuilder.Multiplexing = appSettings?.PostgresMultiplexing ?? false;
postgresConnectionStringBuilder.Database = appSettings?.PostgresDatabase;
postgresConnectionStringBuilder.Username = appSettings?.PostgresUser;
postgresConnectionStringBuilder.Password = appSettings?.PostgresPassword;
builder.Services.AddPooledDbContextFactory<UserServiceDBContext>(
    o => o.UseNpgsql(postgresConnectionStringBuilder.ToString()))
.AddUnitOfWork<UserServiceDBContext>();

//Redis Multiplexer hinzuf�gen wird f�r GraphQL Schema Stitching verwendet
builder.Services.AddSingleton(ConnectionMultiplexer.Connect("redis"));

//Den GraphQL-Server hinzuf�gen
var GraphQLBuilder = builder.Services.AddGraphQLServer()
    .AddDiagnosticEventListener<QueryLogger>()
    .AddMutationType<Mutation>()
    .AddTypeExtension<MutationsUser>()
    .AddQueryType<Query>()
    .AddTypeExtension<QueryUser>()
    .AddTypeExtension<QueryCountry>()
    .AddType<CountryType>()
    .AddType<UserType>()
    .AddFiltering()
    .AddSorting()
    .InitializeOnStartup() //Schema beim Startup initialisieren und nicht beim ersten Request wegen Publish mit Redis
    .PublishSchemaDefinition(c => c
        // The name of the schema. This name should be unique
        .SetName("user")
        .IgnoreRootTypes()
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

//Lokalisierung f�r ASP.NET Core hinzuf�gen
builder.Services.AddLocalization(options => options.ResourcesPath = "Localize");

//Registrieren von MediatR mit der aktuellen Assembly
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

//Festlegen der EndpointConventions f�r MassTransit
Startup.ConfigureEndpointConventions(appSettings);

//Hinzuf�gen der Consumer zum DI-Container
//services.AddScoped<CreateMessageForUserConsumer>();

//Mass-Transit konfigurieren
if (appSettings is not null)
{
    builder.Services.AddMassTransit(x =>
    {
        //Hinzuf�gen der Consumer
        //x.AddConsumer<PictureUploadedConsumer>();

        //RabbitMq hinzuf�gen
        x.UsingRabbitMq((context, cfg) =>
        {
            //Konfigurieren des Hosts
            cfg.Host(new Uri(appSettings.RabbitMQConnection));

            cfg.ReceiveEndpoint(appSettings.Endpoint_UserService, e =>
            {
                e.UseConcurrencyLimit(1);
                e.PrefetchCount = 16;
                e.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20)));

                //e.ConfigureConsumer<PictureUploadedConsumer>(context);
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

    //Routing hinzuf�gen
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

    //Initialisieren der Endpoints f�r GraphQL
    if (builder.Environment.IsDevelopment())
    {
        app.MapGraphQL();
    }
    else
    {
        app.MapGraphQLHttp();
        app.MapGraphQLWebSocket();
    }

    //Initialisieren des Endpoints f�r GraphQL-Voyager
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