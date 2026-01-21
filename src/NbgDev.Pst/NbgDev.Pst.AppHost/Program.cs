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

var web = builder.AddProject<Projects.NbgDev_Pst_Web>("web")
    .WithEnvironment(ctx =>
    {
        if (api.Resource.TryGetEndpoints(out var endpoints))
        {
            var allocatedEndpoints = endpoints
                .Where(e => e.AllocatedEndpoint is not null)
                .Select(e => e.AllocatedEndpoint!)
                .ToList();

            if (allocatedEndpoints.Count > 0)
            {
                var apiUrl = allocatedEndpoints.First().UriString;
                ctx.EnvironmentVariables["PstApi__ApiUrl"] = apiUrl;
            }
        }
    })
    .WithHttpsEndpoint(7004, name: "rest", isProxied: false)
    .WaitFor(api);

builder.Build().Run();
