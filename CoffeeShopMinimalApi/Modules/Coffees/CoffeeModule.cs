using CoffeeShopMinimalApi.Modules.Coffees.Endpoints;
using CoffeeShopMinimalApi.Modules.Coffees.Models;
using CoffeeShopMinimalApi.Modules.Interfaces;

namespace CoffeeShopMinimalApi.Modules.Coffees
{
	public class CoffeeModule : IModule
	{
        public IServiceCollection RegisterModule(IServiceCollection services)
        {
            services.AddScoped<ICoffeeEndpoint, CoffeeEndpoint>();
            return services;
        }

        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            var coffeeItems = endpoints
                .MapGroup(Constants.ModulePath)
                .WithTags(Constants.ModuleEndpointGroupName);


            coffeeItems.MapGet("/", GetAll);
            coffeeItems.MapGet("/{id}", GetById);
            coffeeItems.MapGet("/{id}/ingredients", GetCoffeeIngredients);

            coffeeItems.MapPost("/", Create);
            coffeeItems.MapPut("/{id}", Update);
            coffeeItems.MapDelete("/{id}", Delete);

            return endpoints;
        }

        static Task<IResult> GetAll(ICoffeeEndpoint endponint) => endponint.GetAll();
		static Task<IResult> GetById(ICoffeeEndpoint endponint, Guid id) => endponint.GetById(id);
		static Task<IResult> GetCoffeeIngredients(ICoffeeEndpoint endponint, Guid id) => endponint.GetCoffeeIngredients(id);
		static Task<IResult> Create(ICoffeeEndpoint endponint, CoffeeDto coffee) => endponint.Create(coffee);
		static Task<IResult> Update(ICoffeeEndpoint endponint, Guid id, CoffeeDto coffee) => endponint.Update(id, coffee);
		static Task<IResult> Delete(ICoffeeEndpoint endponint, Guid id) => endponint.Delete(id);
	}
}
