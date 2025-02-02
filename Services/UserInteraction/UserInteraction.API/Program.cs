using GraphQL.Server.Ui.Voyager;
using InfrastructureHelper.EventDispatchHandler;
using Microsoft.AspNetCore.Builder;
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
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;
using System;
using System.Globalization;
using MassTransit;
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

var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Host.UseSerilog(logger);

Log.Logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .CreateBootstrapLogger();

builder.AddServiceDiscovery(options => options.UseEureka());

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddHostedService<EventDispatcherBackgroundService>();

builder.Services.AddSingleton<ILog, LogSerilog>();

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
AppSettings? appSettings = appSettingsSection.Get<AppSettings>();

NpgsqlConnectionStringBuilder postgresConnectionStringBuilder = new()
{
    ApplicationName = "UserInteraction-Service",
    Host = appSettings?.PostgresHost,
    Port = appSettings?.PostgresPort ?? 0,
    Multiplexing = appSettings?.PostgresMultiplexing ?? false,
    Database = appSettings?.PostgresDatabase,
    Username = appSettings?.PostgresUser,
    Password = appSettings?.PostgresPassword
};
builder.Services.AddPooledDbContextFactory<UserInteractionServiceDBContext>(
    o => o.UseNpgsql(postgresConnectionStringBuilder.ToString()))
.AddUnitOfWork<UserInteractionServiceDBContext>();

builder.Services.AddGraphQLServer()
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

builder.Services.AddLocalization(options => options.ResourcesPath = "Localize");

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

Startup.ConfigureEndpointConventions(appSettings);

//builder.Services.AddScoped<MediaItemCreatedConsumer>();

if (appSettings is not null)
{
    builder.Services.AddMassTransit(x =>
    {
        //x.AddConsumer<MediaItemCreatedConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(new Uri(appSettings.RabbitMqConnection));

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
    Log.Information("Starting Web-Host...");

    var app = builder.Build();

    var supportedCultures = new[]
    {
        new CultureInfo("de"),
        new CultureInfo("en")
    };

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

    app.ConfigureExceptionHandler();

    if (appSettings?.PostgresUser != "withoutdocker")
    {
        Startup.InitializeDatabase(app);
    }

    app.UseRouting();

    if (builder.Environment.IsDevelopment())
    {
        app.MapGraphQL();
    }
    else
    {
        app.MapGraphQLHttp();
        app.MapGraphQLWebSocket();
    }

    if (builder.Environment.IsDevelopment())
    {
        app.UseGraphQLVoyager("/graphql-voyager", new VoyagerOptions()
        {
            GraphQLEndPoint = "/graphql"
        });
    }

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
