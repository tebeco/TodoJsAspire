using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TodoJsAspire.ApiService.Db;

var builder = WebApplication.CreateBuilder(args);
///////////////////////////////////////////////////////////////////////////////////////////////////////////
// var connectionString = builder.Configuration.GetConnectionString("TodoDbContext") ?? throw new InvalidOperationException("Connection string 'TodoDbContext' not found.");
// builder.Services.AddDbContext<TodoDbContext>(options => options.UseNpgsql(connectionString));
///////////////////
// builder.Services.AddDbContext<YourDbContext>(options =>
// {
//     options.UseNpgsql(builder.Configuration.GetConnectionString("postgresdb"));
// });
///////////////////
builder.AddAzureNpgsqlDbContext<TodoDbContext>(connectionName: "todojsaspiredb");
///////////////////////////////////////////////////////////////////////////////////////////////////////////

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapDefaultEndpoints();
app.MapTodoEndpoints();

await using(var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.Run();