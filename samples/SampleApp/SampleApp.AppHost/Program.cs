var builder = DistributedApplication.CreateBuilder(args);

Console.Title = "Host: Purview Telemetry Sample App";

builder.AddProject<Projects.SampleApp_Host>("sampleapp-host");

builder.Build().Run();
