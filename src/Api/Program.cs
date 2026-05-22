using Api.Endpoints;
using Api.Helpers;
using Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<CosmosService>();
builder.Services.AddSingleton<UserService>();
// builder.Services.AddScoped<VendorService>();
// builder.Services.AddSingleton<CustomerService>();
builder.Services.AddSingleton<UserQueryHelper>();

var app = builder.Build();


app.MapUserEndpoints();
// app.MapCustomerEndpoints();
// app.MapVendorEndpoints();
app.UseHttpsRedirection();

app.Run();