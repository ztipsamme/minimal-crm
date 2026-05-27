using Api.Endpoints;
using Application.Interfaces;
using Infrastructure.Services;
using Infrastructure.Repo;
using Infrastructure.Helpers;
using Application.Contracts;
using Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<EmailOptions>(
    builder.Configuration.GetSection("Email"));

builder.Services.AddSingleton<CosmosService>();
builder.Services.AddSingleton<EmailService>();
builder.Services.AddSingleton<IUserRepo, UserRepo>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<UserQueryHelper>();

var app = builder.Build();


app.MapUserEndpoints();
app.UseHttpsRedirection();

app.Run();