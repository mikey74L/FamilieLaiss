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
using System.Globalization;
using Upload.API;
using Upload.API.GraphQL.DataLoaders.UploadPicture;
using Upload.API.GraphQL.DataLoaders.UploadVideo;
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
using Upload.API.MassTransit.Consumers.UploadVideo;
using Upload.API.MicroServices;
using Upload.API.Models;
using Upload.API.Services;
using Upload.Infrastructure.DBContext;

Console.Title = "Upload-Service";

var builder = WebApplication.CreateBuilder(args);

//Integrate Aspire
builder
    .AddServiceDefaults()
    .AddAzureBlobClient("blob-storage");
builder
    .AddNpgsqlDbContext<UploadServiceDbContext>("upload-db",
        settings => settings.DisableRetry = true);
    

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

//Adding an HTTPContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//Adding the Hosted-Services (Background-Services)
builder.Services.AddHostedService<EventDispatcherBackgroundService>();

//Adding the global exception handler middleware
builder.Services.AddSingleton<ILog, LogSerilog>();

//Adding Http-Clients for other microservices
builder.Services.AddHttpClient<IGoogleMicroService, GoogleMicroService>(
    client => client.BaseAddress = new("http://google-service"));

//Adding unique identifier service
builder.Services.AddTransient<IUniqueIdentifierGenerator, UniqueIdentifierGeneratorService>();

//Adding Google MicroService
builder.Services.AddSingleton<IGoogleMicroService, GoogleMicroService>();

//Adding the folder helper service
builder.Services.AddSingleton<IFolderHelperService, FolderHelperService>();

//Adding configuration (App-Settings) to the IOC container
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
AppSettings? appSettings = appSettingsSection.Get<AppSettings>();

//Adding pooled context factory and add unit of work
builder.Services.AddPooledDbContextFactory<UploadServiceDbContext>(options =>
    {
    })
    .AddUnitOfWork<UploadServiceDbContext>();

//Create database for Hangfire because hangfire not yet supports aspire database creation
if (!args.Contains("schema"))
{
    var connectionStringHangfire = builder.Configuration.GetConnectionString("upload-hangfire-db");
    var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionStringHangfire);
    var originalDatabaseName = connectionStringBuilder.Database;
    connectionStringBuilder.Database = "postgres";
    using var connection = new NpgsqlConnection(connectionStringBuilder.ToString());
    connection.Open();
    using var checkCommand =
        new NpgsqlCommand($"SELECT 1 FROM pg_database WHERE datname='{originalDatabaseName}'", connection);
    var exists = (int?)checkCommand.ExecuteScalar() == 1;
    if (!exists)
    {
        using var command = new NpgsqlCommand($"CREATE DATABASE \"{originalDatabaseName}\";", connection);
        command.ExecuteNonQuery();
    }

    connection.Close();
}

//Add hangfire 
if (!args.Contains("schema"))
{
    builder.Services.AddHangfire(options =>
        options.UsePostgreSqlStorage(o =>
        {
            o.UseNpgsqlConnection(
                builder.Configuration.GetConnectionString("upload-hangfire-db"));
        }));
    builder.Services.AddTransient<IJobOperations, JobOperationsService>();
    builder.Services.AddTransient<JobExecutorUploadPicture>();
    builder.Services.AddHangfireServer(x => x.ServerTimeout = TimeSpan.FromDays(1));
}

//Adding GraphQL Server
builder.Services.AddGraphQLServer()
    .ModifyCostOptions(o => o.EnforceCostLimits = false)
    .RegisterDbContextFactory<UploadServiceDbContext>()
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
    .InitializeOnStartup();

//Add localization for ASP.NET Core
builder.Services.AddLocalization(options => options.ResourcesPath = "Localize");

//Register MediatR with the current assembly
if (!args.Contains("schema"))
{
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
}

//Setting the EndpointConventions for MassTransit
if (!args.Contains("schema"))
{
    Startup.ConfigureEndpointConventions(appSettings);
}

//Adding the consumers to the DI container
if (!args.Contains("schema"))
{
    builder.Services.AddScoped<MediaItemCreatedConsumer>();
    builder.Services.AddScoped<MediaItemDeletedConsumer>();
    builder.Services.AddScoped<VideoInfoChangedConsumer>();
    builder.Services.AddScoped<VideoConvertedConsumer>();
}

//Adding MassTransit wit RabbitMQ
if (appSettings is not null && !args.Contains("schema"))
{
    builder.Services.AddMassTransit(x =>
    {
        //Hinzufügen der Consumer
        x.AddConsumer<MediaItemCreatedConsumer>();
        x.AddConsumer<MediaItemDeletedConsumer>();
        x.AddConsumer<VideoConvertedConsumer>();
        x.AddConsumer<VideoInfoChangedConsumer>();

        //RabbitMq hinzufügen
        x.UsingRabbitMq((context, cfg) =>
        {
            var configuration = context.GetRequiredService<IConfiguration>();
            var host = configuration.GetConnectionString("RabbitMQConnection");
            cfg.Host(host);

            cfg.ReceiveEndpoint(appSettings.EndpointUploadService, e =>
            {
                e.UseConcurrencyLimit(1);
                e.PrefetchCount = 16;
                e.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20)));

                e.ConfigureConsumer<MediaItemCreatedConsumer>(context);
                e.ConfigureConsumer<MediaItemDeletedConsumer>(context);
                e.ConfigureConsumer<VideoConvertedConsumer>(context);
                e.ConfigureConsumer<VideoInfoChangedConsumer>(context);
            });
        });
    });
}

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

    if (!args.Contains("schema"))
    {
        Log.Information("Initialize and seed database");
        Startup.InitializeDatabase(app);
    }

    Log.Information("Add routing to pipeline");
    app.UseRouting();
    
    Log.Information("Add aspire endpoints to pipeline");
    app.MapDefaultEndpoints();
    
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
    app.RunWithGraphQLCommands(args);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Web app terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}