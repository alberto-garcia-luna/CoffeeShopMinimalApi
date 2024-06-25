using CoffeeShopMinimalApi.Infrastructure.Context;
using CoffeeShopMinimalApi.Infrastructure.Entities;
using MediatR;

namespace CoffeeShopMinimalApi.Infrastructure.Modules.Coffees.Commands
{
	public class DeleteIngredientCommand : IRequest
	{
		public required Ingredient Ingredient { get; set; }
	}

	public class DeleteIngredientCommandHandler : IRequestHandler<DeleteIngredientCommand>
	{
		private readonly CoffeeShopDbContext _context;

		public DeleteIngredientCommandHandler(CoffeeShopDbContext context)
		{
			_context = context;
		}

		public async Task Handle(DeleteIngredientCommand request, CancellationToken cancellationToken)
		{
			_context.Ingredients.Remove(request.Ingredient);
			await _context.SaveChangesAsync();
		}
	}
}
