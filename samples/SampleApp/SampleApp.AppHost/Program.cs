var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.SampleApp_Host>("sampleapp-host");

builder.Build().Run();
