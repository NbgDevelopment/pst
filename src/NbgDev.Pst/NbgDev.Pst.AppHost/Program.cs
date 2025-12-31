using NbgDev.Pst.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("Storage")
    .RunAsEmulator(container =>
    {
        container.WithDataVolume();
    });

var projectStorage = storage.AddTables("Projects");
var apiQueues = storage.AddQueues("ApiQueues");
var processingQueues = storage.AddQueues("ProcessingQueues");

var api = builder.AddProject<Projects.NbgDev_Pst_Api>("nbgdev-pst-api")
    .WithReference(projectStorage)
    .WithReference(apiQueues)
    .WithReference(processingQueues)
    .WaitFor(projectStorage)
    .WaitFor(apiQueues)
    .WaitFor(processingQueues);

var processing = builder.AddProject<Projects.NbgDev_Pst_Processing>("nbgdev-pst-processing")
    .WithReference(apiQueues)
    .WithReference(processingQueues)
    .WaitFor(apiQueues)
    .WaitFor(processingQueues);

builder.AddProject<Projects.NbgDev_Pst_Web>("web", (string?)null)
    .AddWebAssemblyProject("web", api, "PstApi:ApiUrl")
    .WithHttpsEndpoint(7004, name: "rest", isProxied: false)
    .WaitFor(api);

builder.Build().Run();
