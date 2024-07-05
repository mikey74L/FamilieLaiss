﻿using InfrastructureHelper.EventDispatchHandler;
using MassTransit;
using Message.API;
using Message.API.Consumers;
using Message.API.Logging;
using Message.API.Models;
using Message.Infrastructure.DBContext;
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

//Den Titel für das Konsolenfenster setzen
Console.Title = "Message-Service";

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
NpgsqlConnectionStringBuilder postgresConnectionStringBuilder = new();
postgresConnectionStringBuilder.ApplicationName = "Message-Service";
postgresConnectionStringBuilder.Host = appSettings?.PostgresHost;
postgresConnectionStringBuilder.Port = appSettings?.PostgresPort ?? 0;
postgresConnectionStringBuilder.Multiplexing = appSettings?.PostgresMultiplexing ?? false;
postgresConnectionStringBuilder.Database = appSettings?.PostgresDatabase;
postgresConnectionStringBuilder.Username = appSettings?.PostgresUser;
postgresConnectionStringBuilder.Password = appSettings?.PostgresPassword;
builder.Services.AddPooledDbContextFactory<MessageServiceDBContext>(
    o => o.UseNpgsql(postgresConnectionStringBuilder.ToString()))
.AddUnitOfWork<MessageServiceDBContext>();

//Redis Multiplexer hinzufügen wird für GraphQL Schema Stitching verwendet
builder.Services.AddSingleton(ConnectionMultiplexer.Connect("redis"));

//Den GraphQL-Server hinzufügen
//var GraphQLBuilder = builder.Services.AddGraphQLServer()
//    .AddDiagnosticEventListener<QueryLogger>()
//    .AddMutationType<Mutation>()
//    .AddTypeExtension<MutationsCategory>()
//    .AddTypeExtension<MutationsCategoryValue>()
//    .AddTypeExtension<MutationsMediaGroup>()
//    .AddQueryType<Query>()
//    .AddTypeExtension<QueryCategory>()
//    .AddTypeExtension<QueryCategoryValue>()
//    .AddTypeExtension<QueryMediaGroup>()
//    .AddType<CategoryType>()
//    .AddType<CategoryValueType>()
//    .AddType<MediaGroupType>()
//    .AddFiltering()
//    .AddSorting()
//    .InitializeOnStartup() //Schema beim Startup initialisieren und nicht beim ersten Request wegen Publish mit Redis
//    .PublishSchemaDefinition(c => c
//        // The name of the schema. This name should be unique
//        .SetName("catalog")
//        .PublishToRedis(
//            // The configuration name under which the schema should be published
//            "familielaiss",
//            // The connection multiplexer that should be used for publishing
//            sp => sp.GetRequiredService<ConnectionMultiplexer>()
//        )
//    );
//if (!builder.Environment.IsDevelopment())
//{
//    GraphQLBuilder.AddAuthorization();
//}

//Lokalisierung für ASP.NET Core hinzufügen
builder.Services.AddLocalization(options => options.ResourcesPath = "Localize");

//Registrieren von MediatR mit der aktuellen Assembly
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

//Festlegen der EndpointConventions für MassTransit
Startup.ConfigureEndpointConventions(appSettings);

//Hinzufügen der Consumer zum DI-Container
builder.Services.AddScoped<CreateMessageForUserConsumer>();
builder.Services.AddScoped<CreateMessageForGroupConsumer>();

//Mass-Transit konfigurieren
if (appSettings is not null)
{
    builder.Services.AddMassTransit(x =>
    {
        //Hinzufügen der Consumer
        x.AddConsumer<CreateMessageForUserConsumer>();
        x.AddConsumer<CreateMessageForGroupConsumer>();

        //RabbitMq hinzufügen
        x.UsingRabbitMq((context, cfg) =>
        {
            //Konfigurieren des Hosts
            cfg.Host(new Uri(appSettings.RabbitMQConnection));

            cfg.ReceiveEndpoint(appSettings.Endpoint_MessageService, e =>
            {
                e.UseConcurrencyLimit(1);
                e.PrefetchCount = 16;
                e.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20)));

                e.ConfigureConsumer<CreateMessageForUserConsumer>(context);
                e.ConfigureConsumer<CreateMessageForGroupConsumer>(context);
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

    //Authentifizierung verwenden
    if (!builder.Environment.IsDevelopment())
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }

    //Initialisieren der Endpoints für GraphQL
    //app.UseEndpoints(endpoints =>
    //{
    //    if (builder.Environment.IsDevelopment())
    //    {
    //        endpoints.MapGraphQL();
    //    }
    //    else
    //    {
    //        endpoints.MapGraphQLHttp();
    //        endpoints.MapGraphQLWebSocket();
    //    }
    //});

    //Initialisieren des Endpoints für GraphQL-Voyager
    //if (builder.Environment.IsDevelopment())
    //{
    //    app.UseGraphQLVoyager("/graphql-voyager", new VoyagerOptions()
    //    {
    //        GraphQLEndPoint = "/graphql"
    //    });
    //}

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

