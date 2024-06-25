using CoffeeShopMinimalApi.Infrastructure.Context;
using CoffeeShopMinimalApi.Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopMinimalApi.Infrastructure.Modules.Coffees.Queries
{
	public class GetCoffeesQuery : IRequest<GetCoffeesQueryResponse>
	{
	}

	public class GetCoffeesQueryResponse
	{
		public IEnumerable<Coffee>? Coffees { get; set; }
	}

	public class GetCoffeesQueryHandler : IRequestHandler<GetCoffeesQuery, GetCoffeesQueryResponse>
	{
		private readonly CoffeeShopDbContext _context;

		public GetCoffeesQueryHandler(CoffeeShopDbContext context)
		{
			_context = context;
		}

		public async Task<GetCoffeesQueryResponse> Handle(GetCoffeesQuery request, CancellationToken cancellationToken)
		{
			var coffees = await _context.Coffees
				.Include(coffee => coffee.Ingredients)
				.ToArrayAsync();

			return new GetCoffeesQueryResponse
			{
				Coffees = coffees
			};
		}
	}
}
