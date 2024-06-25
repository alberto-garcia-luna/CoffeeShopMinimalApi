namespace CoffeeShopMinimalApi.Modules.Ingredients.Models
{
	public class IngredientDto
	{
		public Guid Id { get; set; }
		public required string Name { get; set; }
	}
}
