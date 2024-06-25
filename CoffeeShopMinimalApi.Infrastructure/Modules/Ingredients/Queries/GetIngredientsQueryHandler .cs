using CoffeeShopMinimalApi.Infrastructure.Context;
using CoffeeShopMinimalApi.Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopMinimalApi.Infrastructure.Modules.Ingredients.Queries
{
	public class GetIngredientsQuery : IRequest<GetIngredientsQueryResponse>
	{
	}

	public class GetIngredientsQueryResponse
	{
		public IEnumerable<Ingredient>? Ingredients { get; set; }
	}

	public class GetIngredientsQueryHandler : IRequestHandler<GetIngredientsQuery, GetIngredientsQueryResponse>
	{
		private readonly CoffeeShopDbContext _context;

		public GetIngredientsQueryHandler(CoffeeShopDbContext context)
		{
			_context = context;
		}

		public async Task<GetIngredientsQueryResponse> Handle(GetIngredientsQuery request, CancellationToken cancellationToken)
		{
			var ingredients = await _context.Ingredients.ToArrayAsync();

			return new GetIngredientsQueryResponse
			{
				Ingredients = ingredients
			};
		}
	}
}
