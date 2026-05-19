using api.Endpoints;
using api.Helpers;
using api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<CosmosService>();
builder.Services.AddScoped<VendorService>();
builder.Services.AddSingleton<CustomerService>();
builder.Services.AddSingleton<CustomerQueryHelper>();

var app = builder.Build();


app.MapCustomerEndpoints();
app.MapVendorEndpoints();
app.UseHttpsRedirection();

app.Run();