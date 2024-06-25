using CoffeeShopMinimalApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopMinimalApi.Infrastructure.Context
{
	public class CoffeeShopDbContext : DbContext
	{
		public CoffeeShopDbContext(DbContextOptions<CoffeeShopDbContext> options) 
			: base(options) { }

		public DbSet<Coffee> Coffees => Set<Coffee>();
		public DbSet<Ingredient> Ingredients => Set<Ingredient>();
	}
}
