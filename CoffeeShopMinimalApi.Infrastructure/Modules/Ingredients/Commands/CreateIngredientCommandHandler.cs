using CoffeeShopMinimalApi.Infrastructure.Context;
using CoffeeShopMinimalApi.Infrastructure.Entities;
using MediatR;

namespace CoffeeShopMinimalApi.Infrastructure.Modules.Ingredients.Commands
{
	public class CreateIngredientCommand : IRequest<CreateIngredientCommandResponse>
	{
		public required Ingredient Ingredient { get; set; }
	}

	public class CreateIngredientCommandResponse
	{
		public required Ingredient Ingredient { get; set; }
	}

	public class CreateIngredientCommandHandler : IRequestHandler<CreateIngredientCommand, CreateIngredientCommandResponse>
	{
		private readonly CoffeeShopDbContext _context;

		public CreateIngredientCommandHandler(CoffeeShopDbContext context)
		{
			_context = context;
		}

		public async Task<CreateIngredientCommandResponse> Handle(CreateIngredientCommand request, CancellationToken cancellationToken)
		{
			_context.Ingredients.Add(request.Ingredient);
			await _context.SaveChangesAsync();

			return new CreateIngredientCommandResponse()
			{
				Ingredient = request.Ingredient
			};
		}
	}
}
