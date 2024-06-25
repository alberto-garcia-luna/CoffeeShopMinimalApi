namespace CoffeeShopMinimalApi.Infrastructure.Entities
{
	public sealed class Coffee
	{
		public Guid Id { get; set; }
		public required string Name { get; set; }
		public required double Price { get; set; }
		public required IEnumerable<Ingredient> Ingredients { get; set; }
		public string? SecretRecipe { get; private set; }
	}
}
