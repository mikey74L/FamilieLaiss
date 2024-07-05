using FlBackEnd.API;
using FlBackEnd.API.Models;
using FLBackEnd.API.GraphQL.Filter;
using FLBackEnd.API.GraphQL.Mutations;
using FLBackEnd.API.GraphQL.Mutations.Category;
using FLBackEnd.API.GraphQL.Mutations.CategoryValue;
using FLBackEnd.API.GraphQL.Mutations.FileUpload;
using FLBackEnd.API.GraphQL.Mutations.MediaGroup;
using FLBackEnd.API.GraphQL.Mutations.MediaItem;
using FLBackEnd.API.GraphQL.Mutations.UploadPicture;
using FLBackEnd.API.GraphQL.Mutations.UploadVideo;
using FLBackEnd.API.GraphQL.Mutations.UserSetting;
using FLBackEnd.API.GraphQL.Policies;
using FLBackEnd.API.GraphQL.Queries;
using FLBackEnd.API.GraphQL.Queries.Category;
using FLBackEnd.API.GraphQL.Queries.CategoryValue;
using FLBackEnd.API.GraphQL.Queries.FileUpload;
using FLBackEnd.API.GraphQL.Queries.Media;
using FLBackEnd.API.GraphQL.Queries.UploadPicture;
using FLBackEnd.API.GraphQL.Queries.UploadVideo;
using FLBackEnd.API.GraphQL.Queries.UserSetting;
using FLBackEnd.API.GraphQL.Subscription;
using FLBackEnd.API.GraphQL.Subscription.PictureConvert;
using FLBackEnd.API.GraphQL.Subscription.VideoConvert;
using FLBackEnd.API.GraphQL.Types.Category;
using FLBackEnd.API.GraphQL.Types.CategoryValue;
using FLBackEnd.API.GraphQL.Types.Media;
using FLBackEnd.API.GraphQL.Types.PictureConvertStatus;
using FLBackEnd.API.GraphQL.Types.UploadPicture;
using FLBackEnd.API.GraphQL.Types.UploadVideo;
using FLBackEnd.API.GraphQL.Types.UserSetting;
using FLBackEnd.API.GraphQL.Types.VideoConvertStatus;
using FLBackEnd.API.Hangfire;
using FLBackEnd.API.Interfaces;
using FLBackEnd.API.Logging;
using FLBackEnd.API.MassTransit.Consumers.PictureConvert;
using FLBackEnd.API.MassTransit.Consumers.VideoConvert;
using FLBackEnd.API.MicroServices;
using FLBackEnd.API.Services;
using FLBackEnd.Infrastructure.DatabaseContext;
using GraphQL.Server.Ui.Voyager;
using Hangfire;
using Hangfire.PostgreSql;
using InfrastructureHelper.EventDispatchHandler;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Serilog;
using ServiceLayerHelper;
using ServiceLayerHelper.Logging;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;
using System.Globalization;
using System.Security.Claims;
using PictureConvertErrorConsumer = FLBackEnd.API.MassTransit.Consumers.PictureConvert.PictureConvertErrorConsumer;

Console.Title = "FlBackEnd-Service";

var builder = WebApplication.CreateBuilder(args);

//Logging
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Host.UseSerilog(logger);

//Create the logger for Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateBootstrapLogger();

//Add Service Discovery
builder.AddServiceDiscovery(options => options.UseEureka());

//Add HttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//Add Hosted Services (Background Services)
//builder.Services.AddHostedService<MessageForAddedMediaItemsBackgroundService>();
builder.Services.AddHostedService<EventDispatcherBackgroundService>();

//Adding unique identifier service
builder.Services.AddTransient<IUniqueIdentifierGenerator, UniqueIdentifierGeneratorService>();

//Adding Google MicroService
builder.Services.AddSingleton<IGoogleMicroService, GoogleMicroService>();

//Add global Exception Handler Middleware
builder.Services.AddSingleton<ILog, LogSerilog>();

//Add Configuration (App-Settings) to IOC Container
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
AppSettings? appSettings = appSettingsSection.Get<AppSettings>();

//Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("FlBackEnd", policy =>
    {
        policy.WithOrigins(appSettings?.CorsOrigin ?? "")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

//Add DB Context Factory including UnitOfWork
NpgsqlConnectionStringBuilder postgresConnectionStringBuilder = [];
postgresConnectionStringBuilder.ApplicationName = "FlBackEnd-Service";
postgresConnectionStringBuilder.Host = appSettings?.PostgresHost;
postgresConnectionStringBuilder.Port = appSettings?.PostgresPort ?? 0;
postgresConnectionStringBuilder.Multiplexing = appSettings?.PostgresMultiplexing ?? false;
postgresConnectionStringBuilder.Database = appSettings?.PostgresDatabase;
postgresConnectionStringBuilder.Username = appSettings?.PostgresUser;
postgresConnectionStringBuilder.Password = appSettings?.PostgresPassword;
builder.Services.AddPooledDbContextFactory<FamilieLaissDbContext>(
        o => o.UseNpgsql(postgresConnectionStringBuilder.ToString()))
    .AddUnitOfWork<FamilieLaissDbContext>();

//Add Authentication and Authorization to container
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.NameIdentifier
    };
});
builder.Services.AddAuthorization(options =>
{
    PoliciesExtensions.ConfigureCategoryPolicies(builder.Environment, options);
    //PoliciesExtensions.ConfigureCategoryValuePolicies(builder.Environment, options);
    PoliciesExtensions.ConfigureMediaGroupPolicies(builder.Environment, options);
});

//Add hangfire 
builder.Services.AddHangfire(options =>
    options.UsePostgreSqlStorage(o => { o.UseNpgsqlConnection(postgresConnectionStringBuilder.ConnectionString); }));
builder.Services.AddTransient<IJobOperations, JobOperationsService>();
builder.Services.AddTransient<JobExecutor>();
builder.Services.AddHangfireServer(x => x.ServerTimeout = TimeSpan.FromDays(1));


//Adding GraphQL Server
var graphQlBuilder = builder.Services.AddGraphQLServer()
    .RegisterDbContext<FamilieLaissDbContext>(DbContextKind.Pooled)
    //.AddDiagnosticEventListener<QueryLogger>()
    .AddMutationType<Mutation>()
    .AddTypeExtension<GeneralQuery>()
    .AddTypeExtension<GraphQlMutationCategory>()
    .AddTypeExtension<GraphQlMutationCategoryValue>()
    .AddTypeExtension<GraphQlMutationFileUpload>()
    .AddTypeExtension<GraphQlMutationUploadPicture>()
    .AddTypeExtension<GraphQlMutationUploadVideo>()
    .AddTypeExtension<GraphQlMutationUserSetting>()
    .AddTypeExtension<GraphQlMutationMediaGroup>()
    .AddTypeExtension<GraphQlMutationMediaItem>()
    .AddQueryType<Query>()
    .AddTypeExtension<GraphQlQueryCategory>()
    .AddTypeExtension<GraphQlQueryCategoryValue>()
    .AddTypeExtension<GraphQlQueryFileUpload>()
    .AddTypeExtension<GraphQlQueryUploadPicture>()
    .AddTypeExtension<GraphQlQueryUploadVideo>()
    .AddTypeExtension<GraphQlQueryUserSetting>()
    .AddTypeExtension<GraphQlQueryMedia>()
    //.AddTypeExtension<QueryMediaItem>()
    .AddType<GraphQlCategoryType>()
    .AddType<GraphQlCategoryValueType>()
    .AddType<GraphQlPictureConvertType>()
    .AddType<GraphQlVideoConvertType>()
    .AddType<GraphQlUploadPictureType>()
    .AddType<GraphQlUploadVideoType>()
    .AddType<GraphQlUserSettingType>()
    .AddType<GraphQlMediaGroupType>()
    .AddType<GraphQlMediaItemType>()
    .AddType<GraphQlMediaItemCategoryValueType>()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .AddSubscriptionType<Subscription>()
    .AddTypeExtension<PictureConvertSubscription>()
    .AddTypeExtension<VideoConvertSubscription>()
    .AddInMemorySubscriptions()
    .AddErrorFilter<GraphQlErrorFilter>()
    .AddAuthorization()
    .InitializeOnStartup();

//Add Localization for ASP.NET Core
builder.Services.AddLocalization(options => options.ResourcesPath = "Localize");

//Register Mediator with the current Assembly
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

//Configure EndpointConventions for MassTransit
Startup.ConfigureEndpointConventions(appSettings);

//Add Consumers to DI Container
builder.Services.AddScoped<PictureConvertedConsumer>();
builder.Services.AddScoped<PictureConvertProgressConsumer>();
builder.Services.AddScoped<PictureConvertErrorConsumer>();
builder.Services.AddScoped<VideoConvertedConsumer>();
builder.Services.AddScoped<VideoConvertProgressConsumer>();
builder.Services.AddScoped<VideoConvertErrorConsumer>();

if (appSettings is not null)
{
    builder.Services.AddMassTransit(x =>
    {
        //Add Consumers
        x.AddConsumer<PictureConvertedConsumer>();
        x.AddConsumer<PictureConvertProgressConsumer>();
        x.AddConsumer<PictureConvertErrorConsumer>();
        x.AddConsumer<VideoConvertedConsumer>();
        x.AddConsumer<VideoConvertProgressConsumer>();
        x.AddConsumer<VideoConvertErrorConsumer>();

        //Add RabbitMq
        x.UsingRabbitMq((context, cfg) =>
        {
            //Configure the Host
            cfg.Host(new Uri(appSettings.RabbitMqConnection));

            cfg.ReceiveEndpoint(appSettings.EndpointFlBackEndService, e =>
            {
                e.UseConcurrencyLimit(1);
                e.PrefetchCount = 16;
                e.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20)));

                e.ConfigureConsumer<PictureConvertedConsumer>(context);
                e.ConfigureConsumer<PictureConvertProgressConsumer>(context);
                e.ConfigureConsumer<PictureConvertErrorConsumer>(context);
                e.ConfigureConsumer<VideoConvertedConsumer>(context);
                e.ConfigureConsumer<VideoConvertProgressConsumer>(context);
                e.ConfigureConsumer<VideoConvertErrorConsumer>(context);
            });
        });
    });
}

//Run the Web Host
try
{
    Log.Information("Starting Web-Host...");

    var app = builder.Build();

    //Add supported languages for the website
    var supportedCultures = new[]
    {
        new CultureInfo("de"),
        new CultureInfo("en")
    };

    //Add Serilog Request Logging
    app.UseSerilogRequestLogging();

    //Add Request Localization to the pipeline based on requests
    app.UseRequestLocalization(new RequestLocalizationOptions
    {
        DefaultRequestCulture = new RequestCulture("en-US"),
        SupportedCultures = supportedCultures,
        SupportedUICultures = supportedCultures
    });

    //Show detailed exception page if in development mode

    if (builder.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    //Add CORS to pipeline
    app.UseCors("FlBackEnd");

    //Configure Exception Handler Middleware
    app.ConfigureExceptionHandler();

    //Initialize the Database (Migration and Seeding)
    Startup.InitializeDatabase(app);

    //Add routing to pipeline
    app.UseRouting();

    //Add Authentication and Authorization to pipeline
    app.UseAuthentication();
    app.UseAuthorization();

    //Add web sockets to pipeline
    app.UseWebSockets();

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