using CoffeeShopMinimalApi.Extensions;
using CoffeeShopMinimalApi.Infrastructure.Extensions;
using CoffeeShopMinimalApi.Modules.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.RegisterServices();
builder.RegisterInfrastructureServices();
builder.Services.RegisterModules();

var app = builder.Build();
app.RegisterMiddlewares();
app.RegisterInfrastructureMiddlewares();
app.MapEndpoints();

app.Run();