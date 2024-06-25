using CoffeeShopMinimalApi.Modules.Ingredients.Models;

namespace CoffeeShopMinimalApi.Modules.Interfaces
{
	public interface IIngredientEndpoint
	{
		public Task<IResult> GetAll();

		public Task<IResult> GetById(Guid id);

		public Task<IResult> Create(IngredientDto coffeeDto);

		public Task<IResult> Update(Guid id, IngredientDto coffeeDto);

		public Task<IResult> Delete(Guid id);
	}
}
