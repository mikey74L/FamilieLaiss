using InfrastructureHelper.EventDispatchHandler;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using ServiceLayerHelper;
using ServiceLayerHelper.Logging;
using System;
using System.Globalization;
using System.Linq;
using UserInteraction.API;
using UserInteraction.API.GraphQl.DataLoaders.Comment;
using UserInteraction.API.GraphQl.DataLoaders.Favorite;
using UserInteraction.API.GraphQl.DataLoaders.MediaItem;
using UserInteraction.API.GraphQl.DataLoaders.Rating;
using UserInteraction.API.GraphQl.DataLoaders.UserAccount;
using UserInteraction.API.GraphQl.DataLoaders.UserInteraction;
using UserInteraction.API.GraphQl.Filter;
using UserInteraction.API.GraphQl.Queries;
using UserInteraction.API.GraphQl.Queries.Comment;
using UserInteraction.API.GraphQl.Queries.Favorite;
using UserInteraction.API.GraphQl.Queries.MediaItem;
using UserInteraction.API.GraphQl.Queries.Rating;
using UserInteraction.API.GraphQl.Queries.UserAccount;
using UserInteraction.API.GraphQl.Queries.UserInteraction;
using UserInteraction.API.GraphQl.Types.Comment;
using UserInteraction.API.GraphQl.Types.Favorite;
using UserInteraction.API.GraphQl.Types.MediaItem;
using UserInteraction.API.GraphQl.Types.Rating;
using UserInteraction.API.GraphQl.Types.UserAccount;
using UserInteraction.API.GraphQl.Types.UserInteractionInfo;
using UserInteraction.API.Logging;
using UserInteraction.API.Models;
using UserInteraction.Infrastructure.DBContext;

Console.Title = "UserInteraction-Service";

var builder = WebApplication.CreateBuilder(args);

//Integrate Aspire
builder
    .AddServiceDefaults()
    .AddNpgsqlDbContext<UserInteractionServiceDBContext>("user-interaction-db",
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
builder.Services.AddPooledDbContextFactory<UserInteractionServiceDBContext>(options =>
    {
    })
    .AddUnitOfWork<UserInteractionServiceDBContext>();

//Adding GraphQL Server
builder.Services.AddGraphQLServer()
    .ModifyCostOptions(o => o.EnforceCostLimits = false)
    .RegisterDbContextFactory<UserInteractionServiceDBContext>()
//    .AddMutationType<Mutation>()
//    .AddTypeExtension<GraphQlMutationCategory>()
//    .AddTypeExtension<GraphQlMutationCategoryValue>()
//    .AddTypeExtension<GraphQlMutationMediaGroup>()
//    .AddTypeExtension<GraphQlMutationMediaItem>()
    .AddQueryType<Query>()
    .AddTypeExtension<GraphQlQueryComment>()
    .AddTypeExtension<GraphQlQueryFavorite>()
    .AddTypeExtension<GraphQlQueryRating>()
    .AddTypeExtension<GraphQlQueryUserInteraction>()
    .AddTypeExtension<GraphQlQueryUserAccount>()
    .AddTypeExtension<GraphQlQueryMediaItem>()
    .AddType<GraphQlRatingType>()
    .AddType<GraphQlCommentType>()
    .AddType<GraphQlFavoriteType>()
    .AddType<GraphQlMediaItemType>()
    .AddType<GraphQlUserAccountType>()
    .AddType<GraphQlUserInteractionInfoType>()
    .AddDataLoader<CommentDataLoader>()
    .AddDataLoader<FavoriteDataLoader>()
    .AddDataLoader<RatingDataLoader>()
    .AddDataLoader<UserAccountDataLoader>()
    .AddDataLoader<UserInteractionDataLoader>()
    .AddDataLoader<MediaItemDataLoader>()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .AddErrorFilter<GraphQlErrorFilter>()
//    .AddAuthorization()
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
//builder.Services.AddScoped<MediaItemCreatedConsumer>();

//Adding MassTransit with RabbitMQ
if (appSettings is not null && !args.Contains("schema"))
{
    builder.Services.AddMassTransit(x =>
    {
        //x.AddConsumer<MediaItemCreatedConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            var configuration = context.GetRequiredService<IConfiguration>();
            var host = configuration.GetConnectionString("RabbitMQConnection");
            cfg.Host(host);

            cfg.ReceiveEndpoint(appSettings.EndpointUserInteractionService, e =>
            {
                e.UseConcurrencyLimit(1);
                e.PrefetchCount = 16;
                e.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20)));

                //e.ConfigureConsumer<MediaItemCreatedConsumer>(context);
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
