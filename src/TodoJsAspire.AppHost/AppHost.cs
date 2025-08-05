var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var postgres = builder
        .AddAzurePostgresFlexibleServer("postgres")
        .RunAsContainer(pgBuilder =>
        {
            pgBuilder.WithLifetime(ContainerLifetime.Persistent);
            pgBuilder.WithDataVolume();
            pgBuilder.WithPgAdmin();
        });

var postgresdb = postgres.AddDatabase("postgresdb");

var apiService = builder.AddProject<Projects.TodoJsAspire_ApiService>("apiservice")
    .WithReference(postgresdb)
    .WithHttpHealthCheck("/health");

// builder.AddProject<Projects.TodoJsAspire_Web>("webfrontend")
//     .WithExternalHttpEndpoints()
//     .WithHttpHealthCheck("/health")
//     .WithReference(cache)
//     .WaitFor(cache)
//     .WithReference(apiService)
//     .WaitFor(apiService);

builder.Build().Run();
