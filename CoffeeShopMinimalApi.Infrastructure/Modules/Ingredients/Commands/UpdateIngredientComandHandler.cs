using CoffeeShopMinimalApi.Infrastructure.Context;
using CoffeeShopMinimalApi.Infrastructure.Entities;
using MediatR;

namespace CoffeeShopMinimalApi.Infrastructure.Modules.Coffees.Commands
{
	public class UpdateIngredientCommand : IRequest
	{
		public required Ingredient Ingredient { get; set; }
	}

	public class UpdateIngredientCommandHandler : IRequestHandler<UpdateIngredientCommand>
	{
		private readonly CoffeeShopDbContext _context;

		public UpdateIngredientCommandHandler(CoffeeShopDbContext context)
		{
			_context = context;
		}

		public async Task Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
		{
			_context.Ingredients.Update(request.Ingredient);
			await _context.SaveChangesAsync();
		}
	}
}
