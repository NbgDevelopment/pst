using NbgDev.Pst.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.NbgDev_Pst_Api>("nbgdev-pst-api");

builder.AddWebAssemblyProject<Projects.NbgDev_Pst_Web>("web", api);

builder.Build().Run();
