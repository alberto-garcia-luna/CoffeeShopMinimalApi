using CoffeeShopMinimalApi.Infrastructure.Context;
using CoffeeShopMinimalApi.Infrastructure.Entities;
using MediatR;

namespace CoffeeShopMinimalApi.Infrastructure.Modules.Ingredients.Queries
{
	public class GetIngredientQuery : IRequest<GetIngredientQueryResponse>
	{
		public required Guid Id { get; set; }
	}

	public class GetIngredientQueryResponse
	{
		public Ingredient? Ingredient { get; set; }
	}

	public class GetIngredientQueryHandler : IRequestHandler<GetIngredientQuery, GetIngredientQueryResponse>
	{
		private readonly CoffeeShopDbContext _context;

		public GetIngredientQueryHandler(CoffeeShopDbContext context)
		{
			_context = context;
		}

		public async Task<GetIngredientQueryResponse> Handle(GetIngredientQuery request, CancellationToken cancellationToken)
		{
			var ingredient = await _context.Ingredients.FindAsync(request.Id);
			return new GetIngredientQueryResponse
			{
				Ingredient = ingredient
			};
		}
	}
}
