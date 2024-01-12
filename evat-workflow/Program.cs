
using evat_workflow;

var builder = WebApplication.CreateBuilder(args);

var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Configuration
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{enviroment}.json", true, true)
    .AddJsonFile($"appsettings.{Environment.MachineName}.json", true, true)
    .AddEnvironmentVariables()
    .Build();


var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services); // calling ConfigureServices method


var app = builder.Build();
startup.Configure(app, builder.Environment); // calling Configure method