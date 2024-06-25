using CoffeeShopMinimalApi.Infrastructure.Context;
using CoffeeShopMinimalApi.Infrastructure.Entities;
using MediatR;

namespace CoffeeShopMinimalApi.Infrastructure.Modules.Coffees.Commands
{
	public class CreateCoffeeCommand : IRequest<CreateCoffeeCommandResponse>
	{
		public required Coffee Coffee { get; set; }
	}

	public class CreateCoffeeCommandResponse
	{
		public required Coffee Coffee { get; set; }
	}

	public class CreateCoffeeCommandHandler : IRequestHandler<CreateCoffeeCommand, CreateCoffeeCommandResponse>
	{
		private readonly CoffeeShopDbContext _context;

		public CreateCoffeeCommandHandler(CoffeeShopDbContext context) 
		{
			_context = context;
		}

		public async Task<CreateCoffeeCommandResponse> Handle(CreateCoffeeCommand request, CancellationToken cancellationToken)
		{
			_context.Coffees.Add(request.Coffee);
			await _context.SaveChangesAsync();

			return new CreateCoffeeCommandResponse() 
			{ 
				Coffee = request.Coffee
			};
		}
	}
}
