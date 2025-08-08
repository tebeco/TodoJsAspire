var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var postgres = builder
        .AddAzurePostgresFlexibleServer("postgresdb")
        .RunAsContainer(pgBuilder =>
        {
            pgBuilder
                .WithLifetime(ContainerLifetime.Persistent)
                .WithDataVolume("todojsaspire_postgres_data");

            pgBuilder
                .WithPgAdmin()
                .WithLifetime(ContainerLifetime.Persistent);
        });

var todoJsAspireDb = postgres.AddDatabase("todojsaspiredb", "todojsaspire");

var apiService = builder.AddProject<Projects.TodoJsAspire_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithReference(todoJsAspireDb)
    .WaitFor(todoJsAspireDb);

var frontend = builder.AddViteApp("todo-frontend", workingDirectory: "../../todo-frontend")
        .WithReference(apiService)
        .WaitFor(apiService)
        .WithPnpmPackageInstallation();

builder.Build().Run();
