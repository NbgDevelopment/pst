var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.NbgDev_Pst_Web>("web");

builder.Build().Run();
