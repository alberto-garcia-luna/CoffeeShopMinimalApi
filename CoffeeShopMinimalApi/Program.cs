using CoffeeShopMinimalApi.Infrastructure.Extensions;
using CoffeeShopMinimalApi.Extensions;
using CoffeeShopMinimalApi.Modules.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.RegisterServices();
builder.RegisterInfrastructureServices();
builder.Services.RegisterModules();

var app = builder.Build();
app.RegisterMiddlewares();
app.MapEndpoints();

app.Run();