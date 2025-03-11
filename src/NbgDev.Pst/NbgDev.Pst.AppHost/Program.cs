using NbgDev.Pst.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("Storage")
    .RunAsEmulator();

var projectStorage = storage.AddTables("Projects");

var api = builder.AddProject<Projects.NbgDev_Pst_Api>("nbgdev-pst-api")
    .WithReference(projectStorage)
    .WaitFor(projectStorage);

builder.AddProject<Projects.NbgDev_Pst_Web>("web", (string?)null)
    .AddWebAssemblyProject("web", api, "PstApi:ApiUrl")
    .WithHttpsEndpoint(7004, name: "rest", isProxied: false)
    .WaitFor(api);

builder.Build().Run();
