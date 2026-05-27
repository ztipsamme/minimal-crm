using Application.Interfaces;
using Application.Services;
using Infrastructure.Helpers;
using Infrastructure.Repo;
using Infrastructure.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.AddSingleton<CosmosService>();
builder.Services.AddSingleton<EmailService>();
builder.Services.AddSingleton<IUserRepo, UserRepo>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<UserQueryHelper>();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
