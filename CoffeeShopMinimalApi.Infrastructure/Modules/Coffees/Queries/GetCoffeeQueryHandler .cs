﻿using CoffeeShopMinimalApi.Infrastructure.Context;
using CoffeeShopMinimalApi.Infrastructure.Entities;
using MediatR;

namespace CoffeeShopMinimalApi.Infrastructure.Modules.Coffees.Queries
{
	public class GetCoffeeQuery : IRequest<GetCoffeeQueryResponse>
	{
		public required Guid Id { get; set; }
	}

	public class GetCoffeeQueryResponse
	{
		public Coffee? Coffee { get; set; }
	}

	public class GetCoffeeQueryHandler : IRequestHandler<GetCoffeeQuery, GetCoffeeQueryResponse>
	{
		private readonly CoffeeShopDbContext _context;

        public GetCoffeeQueryHandler(CoffeeShopDbContext context)
        {
			_context = context;
		}

		public async Task<GetCoffeeQueryResponse> Handle(GetCoffeeQuery request, CancellationToken cancellationToken)
		{
			var coffee = await _context.Coffees.FindAsync(request.Id);
			return new GetCoffeeQueryResponse
			{
				Coffee = coffee
			};
		}
	}
}