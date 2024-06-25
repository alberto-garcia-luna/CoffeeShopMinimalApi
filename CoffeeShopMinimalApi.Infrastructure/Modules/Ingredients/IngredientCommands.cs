using CoffeeShopMinimalApi.Infrastructure.Context;
using CoffeeShopMinimalApi.Infrastructure.Entities;

namespace CoffeeShopMinimalApi.Infrastructure.Modules.Ingredients
{
	public class IngredientCommands
	{
		private readonly CoffeeShopDbContext _context;

		public IngredientCommands(CoffeeShopDbContext context)
		{
			_context = context;
		}

		public async Task Create(Ingredient ingredient)
		{
			_context.Ingredients.Add(ingredient);
			await _context.SaveChangesAsync();
		}

		public async Task Update(Ingredient ingredient)
		{
			_context.Ingredients.Update(ingredient);
			await _context.SaveChangesAsync();
		}

		public async Task Delete(Ingredient ingredient)
		{
			_context.Ingredients.Remove(ingredient);
			await _context.SaveChangesAsync();
		}
	}
}
