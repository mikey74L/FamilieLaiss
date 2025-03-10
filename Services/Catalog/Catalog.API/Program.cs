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
using InfrastructureHelper.EventDispatchHandler;
using MassTransit;
using Microsoft.AspNetCore.Localization;
using Serilog;
using ServiceLayerHelper;
using ServiceLayerHelper.Logging;
using System.Globalization;

Console.Title = "Catalog-Service";

var builder = WebApplication.CreateBuilder(args);

//Integrate Aspire
builder
    .AddServiceDefaults()
    .AddNpgsqlDbContext<CatalogServiceDbContext>("catalog-db", 
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

//Adding configuration (App-Settings) to the IOC container
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();

//Adding pooled context factory and add unit of work
builder.Services.AddPooledDbContextFactory<CatalogServiceDbContext>(options =>
    {
    })
    .AddUnitOfWork<CatalogServiceDbContext>();

//Adding GraphQL Server
builder.Services.AddGraphQLServer()
    .ModifyCostOptions(o => o.EnforceCostLimits = false)
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
    builder.Services.AddScoped<UploadPictureCreatedConsumer>();
    builder.Services.AddScoped<UploadVideoCreatedConsumer>();
    builder.Services.AddScoped<UploadPictureDeletedConsumer>();
    builder.Services.AddScoped<UploadVideoDeletedConsumer>();
}

//Adding MassTransit wit RabbitMQ
if (appSettings is not null && !args.Contains("schema"))
{
    builder.Services.AddMassTransit(x =>
    {
        x.AddConsumer<UploadPictureCreatedConsumer>();
        x.AddConsumer<UploadVideoCreatedConsumer>();
        x.AddConsumer<UploadPictureDeletedConsumer>();
        x.AddConsumer<UploadVideoDeletedConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            var configuration = context.GetRequiredService<IConfiguration>();
            var host = configuration.GetConnectionString("RabbitMQConnection");
            cfg.Host(host);

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
    if (builder.Environment.IsDevelopment())
    {
        app.MapGraphQL();
    }
    else
    {
        app.MapGraphQLHttp();
        app.MapGraphQLWebSocket();
    }
    
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