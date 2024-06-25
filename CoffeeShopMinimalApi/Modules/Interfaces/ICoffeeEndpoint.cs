using CoffeeShopMinimalApi.Modules.Coffees.Models;

namespace CoffeeShopMinimalApi.Modules.Interfaces
{
	public interface ICoffeeEndpoint
	{
		public Task<IResult> GetAll();

		public Task<IResult> GetById(Guid id);

		public Task<IResult> GetCoffeeIngredients(Guid id);

		public Task<IResult> Create(CoffeeDto coffeeDto);

		public Task<IResult> Update(Guid id, CoffeeDto coffeeDto);

		public Task<IResult> Delete(Guid id);
	}
}
