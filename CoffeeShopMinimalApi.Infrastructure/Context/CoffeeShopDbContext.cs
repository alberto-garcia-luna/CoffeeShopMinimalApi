using CoffeeShopMinimalApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopMinimalApi.Infrastructure.Context
{
	public class CoffeeShopDbContext : DbContext
	{
        public CoffeeShopDbContext()
        {            
        }

        public CoffeeShopDbContext(DbContextOptions<CoffeeShopDbContext> options) 
			: base(options) { }

		public virtual DbSet<Coffee> Coffees => Set<Coffee>();
		public virtual DbSet<Ingredient> Ingredients => Set<Ingredient>();
	}
}
