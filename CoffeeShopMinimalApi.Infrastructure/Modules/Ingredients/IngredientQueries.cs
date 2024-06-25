using CoffeeShopMinimalApi.Infrastructure.Context;
using CoffeeShopMinimalApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopMinimalApi.Infrastructure.Modules.Ingredients
{
	public class IngredientQueries
	{
		private readonly CoffeeShopDbContext _context;

        public IngredientQueries(CoffeeShopDbContext context)
        {
			_context = context;
		}

		public async Task<IEnumerable<Ingredient>> GetAll()
		{
			return await _context.Ingredients
				.ToArrayAsync();
		}

		public async Task<Ingredient> GetById(Guid id)
		{
			return await _context.Ingredients
				.FirstOrDefaultAsync(coffee => coffee.Id == id);
		}
	}
}
