namespace CoffeeShopMinimalApi.Infrastructure.Entities
{
	public sealed class Ingredient
	{
		public Guid Id { get; set; }
		public required string Name { get; set; }
	}
}
