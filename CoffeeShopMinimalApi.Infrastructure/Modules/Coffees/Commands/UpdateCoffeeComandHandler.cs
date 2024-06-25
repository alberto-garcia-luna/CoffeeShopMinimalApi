using CoffeeShopMinimalApi.Infrastructure.Context;
using CoffeeShopMinimalApi.Infrastructure.Entities;
using MediatR;

namespace CoffeeShopMinimalApi.Infrastructure.Modules.Coffees.Commands
{
	public class UpdateCoffeeCommand : IRequest
	{
		public required Coffee Coffee { get; set; }
	}

	public class UpdateCoffeeCommandHandler : IRequestHandler<UpdateCoffeeCommand>
	{
		private readonly CoffeeShopDbContext _context;

		public UpdateCoffeeCommandHandler(CoffeeShopDbContext context)
		{
			_context = context;
		}

		public async Task Handle(UpdateCoffeeCommand request, CancellationToken cancellationToken)
		{
			_context.Coffees.Update(request.Coffee);
			await _context.SaveChangesAsync();
		}
	}
}
