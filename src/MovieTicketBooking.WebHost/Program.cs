using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MovieTicketBooking.WebHost.Extensions;
using MovieTicketBooking.WebHost.Middleware;
using MovieTicketBooking.Common.Presentation.Endpoints;
using RabbitMQ.Client;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(t => t.FullName?.Replace("+", "."));
});

builder.AddLogging();

//builder.Configuration.AddModuleConfiguration([
//   // add module env here
//]);

var databaseConnectionString = builder.Configuration.GetConnectionString("Database")!;
var cacheConnectionString = builder.Configuration.GetConnectionString("Cache")!;
var rabbitMqConnectionString = builder.Configuration.GetConnectionString("Queue")!;

builder.Services
    .AddExceptionHandling()
    .AddOpenApi();

//builder.Services
//    .AddModules(
//        builder.Configuration,
//        databaseConnectionString,
//        cacheConnectionString);

var keycloakHealthUrl = builder.Configuration.GetValue<string>("KeyCloak:HealthUrl")!;

//builder.Services
//    .AddHealthChecks()
//    .AddNpgSql(databaseConnectionString)
//    .AddRedis(cacheConnectionString)
//    .AddRabbitMQ(rabbitConnectionString: rabbitMqConnectionString)
//    .AddUrlGroup(new Uri(keycloakHealthUrl), HttpMethod.Get, "keycloak");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}

app.MapEndpoints();

//app.MapHealthChecks("health", new HealthCheckOptions
//{
//    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
//});

app.UseLogContextTraceLogging();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

//app.UseAuthentication();

//app.UseAuthorization();

await app.RunAsync();

public abstract partial class Program;