using Microsoft.Extensions.Configuration;
using System.Reflection;

var builder = DistributedApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());

var keyVault = builder.ExecutionContext.IsPublishMode ?
    builder.AddAzureKeyVault("key-vault") : builder.AddConnectionString("key-vault");

var blobStorage = builder.AddAzureStorage("azure-storage")
    .RunAsEmulator(emulator =>
    {
        emulator.WithLifetime(ContainerLifetime.Persistent)
                .WithDataVolume()
                .WithArgs("azurite", "-l", "/data", "--blobHost", "0.0.0.0", "--queueHost", "0.0.0.0", "--tableHost", "0.0.0.0", "--skipApiVersionCheck");
    })
    .AddBlobs("blob-storage");

//Fügt zur Entwicklungszeit einen Container hinzu welcher einen
//auf Blazor basierenden Storage-Explorer bereitstellt, der auf den Storage-Emulator zugreift
builder.AddContainer("storage-explorer", "sebagomez/azurestorageexplorer")
    .WithEndpoint(scheme: "http", targetPort: 8080, port: 42684)
    .WithEnvironment(e =>
    {
        e.EnvironmentVariables["AZURE_STORAGE_CONNECTIONSTRING"] = new ConnectionStringReference(blobStorage.Resource, optional: false);
    })
    .ExcludeFromManifest();

var usernameRabbit = builder.AddParameter("usernameRabbit", secret: true);
var passwordRabbit = builder.AddParameter("passwordRabbit", secret: true);
var messagingRabbit = builder.AddRabbitMQ("RabbitMQConnection",
        usernameRabbit, passwordRabbit)
    .WithDataVolume()
    .WithManagementPlugin();

var usernamePostgres = builder.AddParameter("usernamePostgres", secret: true);
var passwordPostgres = builder.AddParameter("passwordPostgres", secret: true);
var postgres = builder.AddPostgres("postgres",
        usernamePostgres, passwordPostgres)
    .WithDataVolume()
    .WithPgAdmin();


var postgresDbCatalog = postgres.AddDatabase("catalog-db");
var postgresDbSettings = postgres.AddDatabase("settings-db");
var postgresDbUser = postgres.AddDatabase("user-db");
var postgresUserInteraction = postgres.AddDatabase("user-interaction-db");
var postgresDbUpload = postgres.AddDatabase("upload-db");
var postgresDbUploadHangfire = postgres.AddDatabase("upload-hangfire-db");

var catalogService = builder.AddProject<Projects.Catalog_API>("catalog-service")
    .WithReference(messagingRabbit)
    .WithReference(postgresDbCatalog)
    .WaitFor(messagingRabbit)
    .WaitFor(postgresDbCatalog);

var settingsService = builder.AddProject<Projects.Settings_API>("settings-service")
    .WithReference(messagingRabbit)
    .WithReference(postgresDbSettings)
    .WaitFor(messagingRabbit)
    .WaitFor(postgresDbSettings);

var userService = builder.AddProject<Projects.User_API>("user-service")
    .WithReference(messagingRabbit)
    .WithReference(postgresDbUser)
    .WaitFor(messagingRabbit)
    .WaitFor(postgresDbUser);

var userInteractionService = builder.AddProject<Projects.UserInteraction_API>("user-interaction-service")
    .WithReference(messagingRabbit)
    .WithReference(postgresUserInteraction)
    .WaitFor(messagingRabbit)
    .WaitFor(postgresUserInteraction);

var googleService = builder.AddProject<Projects.Google_API>("google-service")
    .WithReference(keyVault)
    .WithReference(messagingRabbit)
    .WaitFor(messagingRabbit);

var uploadService = builder.AddProject<Projects.Upload_API>("upload-service")
    .WithReference(messagingRabbit)
    .WithReference(postgresDbUpload)
    .WithReference(postgresDbUploadHangfire)
    .WithReference(googleService)
    .WithReference(blobStorage)
    .WaitFor(messagingRabbit)
    .WaitFor(postgresDbUpload)
    .WaitFor(postgresDbUploadHangfire)
    .WaitFor(googleService)
    .WaitFor(blobStorage);

var gateway = builder.AddFusionGateway<Projects.SPAGateway>("gateway")
    .WithSubgraph(catalogService)
    .WithSubgraph(settingsService)
    .WithSubgraph(userService)
    .WithSubgraph(userInteractionService)
    .WithSubgraph(googleService)
    .WithSubgraph(uploadService);

var frontEnd = builder.AddProject<Projects.FamilieLaissFrontend>("frontend");

builder.Build().Compose().Run();