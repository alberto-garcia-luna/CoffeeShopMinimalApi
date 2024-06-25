using CoffeeShopMinimalApi.Modules.Ingredients.Models;

namespace CoffeeShopMinimalApi.Modules.Coffees.Models
{
	public class CoffeeDto
	{
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required double Price { get; set; }
        public required IEnumerable<IngredientDto> Ingredients { get; set; }
    }
}
