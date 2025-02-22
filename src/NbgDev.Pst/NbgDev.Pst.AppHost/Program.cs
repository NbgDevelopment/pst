using NbgDev.Pst.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.NbgDev_Pst_Api>("nbgdev-pst-api");

builder.AddProject<Projects.NbgDev_Pst_Web>("web", (string?)null)
    .AddWebAssemblyProject("web", api, "PstApi:ApiUrl")
    .WithHttpsEndpoint(7004, name: "rest", isProxied: false);

builder.Build().Run();
