using CoffeeShopMinimalApi.Infrastructure.Entities;

namespace CoffeeShopMinimalApi.Tests
{
	static internal class TestDataGenerator
	{
		public static IList<Ingredient> GetTestIngredients()
		{
			return [
				new Ingredient()
				{
					Id = Guid.NewGuid(),
					Name = "Roasted Coffee"
				},
				new Ingredient()
				{
					Id = Guid.NewGuid(),
					Name = "Milk"
				},
				new Ingredient()
				{
					Id = Guid.NewGuid(),
					Name = "Water"
				},
				new Ingredient()
				{
					Id = Guid.NewGuid(),
					Name = "Espresso Coffee"
				}
			];
		}

		public static IList<Coffee> GetTestCoffees() 
		{
			return [
				new Coffee()
				{
					Id = Guid.NewGuid(),
					Name = "Cappuccino",
					Price = 50,
					Ingredients = [
						new Ingredient()
						{
							Id = Guid.NewGuid(),
							Name = "Roasted Coffee"
						},
						new Ingredient()
						{
							Id = Guid.NewGuid(),
							Name = "Milk"
						},
						new Ingredient()
						{
							Id = Guid.NewGuid(),
							Name = "Water"
						}
					]
				},
				new Coffee()
				{
					Id = Guid.NewGuid(),
					Name = "Latte",
					Price = 50,
					Ingredients = [
						new Ingredient()
						{
							Id = Guid.NewGuid(),
							Name = "Espresso Coffee"
						},
						new Ingredient()
						{
							Id = Guid.NewGuid(),
							Name = "Milk"
						}
					]
				}
			];
		}
	}
}
