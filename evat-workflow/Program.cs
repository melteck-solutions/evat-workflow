
using evat_workflow;

var builder = WebApplication.CreateBuilder(args);

var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Configuration
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{enviroment}.json", true, true)
    .AddJsonFile($"appsettings.{Environment.MachineName}.json", true, true)
    .AddEnvironmentVariables()
    .Build();


var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app, builder.Environment);