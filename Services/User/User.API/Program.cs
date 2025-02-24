using InfrastructureHelper.EventDispatchHandler;
using MassTransit;
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
using System;
using System.Globalization;
using System.Linq;
using User.API;
using User.API.GraphQL.DataLoaders.Country;
using User.API.GraphQL.DataLoaders.UserAccount;
using User.API.GraphQL.Filter;
using User.API.GraphQL.Mutations;
using User.API.GraphQL.Mutations.UserMutations;
using User.API.GraphQL.Queries;
using User.API.GraphQL.Queries.Country;
using User.API.GraphQL.Queries.UserQuery;
using User.API.GraphQL.Types;
using User.API.Logging;
using User.API.Models;
using User.Infrastructure.DBContext;

Console.Title = "User-Service";

var builder = WebApplication.CreateBuilder(args);

//Integrate Aspire
builder
    .AddServiceDefaults()
    .AddNpgsqlDbContext<UserServiceDbContext>("user-db",
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
builder.Services.AddPooledDbContextFactory<UserServiceDbContext>(options =>
    {
    })
    .AddUnitOfWork<UserServiceDbContext>();

//Adding GraphQL Server
builder.Services.AddGraphQLServer()
    .ModifyCostOptions(o => o.EnforceCostLimits = false)
    .RegisterDbContextFactory<UserServiceDbContext>()
    .AddMutationType<Mutation>()
    .AddTypeExtension<MutationsUser>()
    .AddQueryType<Query>()
    .AddTypeExtension<GraphQlQueryUser>()
    .AddTypeExtension<GraphQlQueryCountry>()
    .AddType<CountryType>()
    .AddType<UserType>()
    .AddDataLoader<CountryDataLoader>()
    .AddDataLoader<UserAccountDataLoader>()
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
//services.AddScoped<CreateMessageForUserConsumer>();

//Adding MassTransit with RabbitMQ
if (appSettings is not null && !args.Contains("schema"))
{
    builder.Services.AddMassTransit(x =>
    {
        //x.AddConsumer<PictureUploadedConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            var configuration = context.GetRequiredService<IConfiguration>();
            var host = configuration.GetConnectionString("RabbitMQConnection");
            cfg.Host(host);

            cfg.ReceiveEndpoint(appSettings.EndpointUserService, e =>
            {
                e.UseConcurrencyLimit(1);
                e.PrefetchCount = 16;
                e.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20)));

                //e.ConfigureConsumer<PictureUploadedConsumer>(context);
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
        Startup.SeedDatabase(app);
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
