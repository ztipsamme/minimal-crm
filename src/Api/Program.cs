using api.Endpoints;
using api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<CosmosService>();
builder.Services.AddScoped<VendorService>();
builder.Services.AddSingleton<CustomerService>();

var app = builder.Build();


app.MapCustomerEndpoints();
app.MapVendorEndpoints();
app.UseHttpsRedirection();

app.Run();