using CoffeeShopMinimalApi.Modules.Ingredients.Endpoints;
using CoffeeShopMinimalApi.Modules.Ingredients.Models;
using CoffeeShopMinimalApi.Modules.Interfaces;

namespace CoffeeShopMinimalApi.Modules.Ingredients
{
	public class IngredientModule : IModule
    {
        public IServiceCollection RegisterModule(IServiceCollection services)
        {
			services.AddScoped<IIngredientEndpoint, IngredientEndpoint>();
			return services;
        }

        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            var ingredientItems = endpoints
                .MapGroup(Constants.ModulePath)
                .WithTags(Constants.ModuleEndpointGroupName);

            ingredientItems.MapGet("/", GetAll);
            ingredientItems.MapGet("/{id}", GetById);

            ingredientItems.MapPost("/", Create);
            ingredientItems.MapPut("/{id}", Update);
            ingredientItems.MapDelete("/{id}", Delete);

            return endpoints;
        }

		static Task<IResult> GetAll(IIngredientEndpoint endponint) => endponint.GetAll();
		static Task<IResult> GetById(IIngredientEndpoint endponint, Guid id) => endponint.GetById(id);
		static Task<IResult> Create(IIngredientEndpoint endponint, IngredientDto coffee) => endponint.Create(coffee);
		static Task<IResult> Update(IIngredientEndpoint endponint, Guid id, IngredientDto coffee) => endponint.Update(id, coffee);
		static Task<IResult> Delete(IIngredientEndpoint endponint, Guid id) => endponint.Delete(id);
	}
}
