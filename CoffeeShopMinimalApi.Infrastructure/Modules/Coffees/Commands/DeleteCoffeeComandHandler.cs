using CoffeeShopMinimalApi.Infrastructure.Context;
using CoffeeShopMinimalApi.Infrastructure.Entities;
using MediatR;

namespace CoffeeShopMinimalApi.Infrastructure.Modules.Coffees.Commands
{
	public class DeleteCoffeeCommand : IRequest
	{
		public required Coffee Coffee { get; set; }
	}

	public class DeleteCoffeeCommandHandler : IRequestHandler<DeleteCoffeeCommand>
	{
		private readonly CoffeeShopDbContext _context;

		public DeleteCoffeeCommandHandler(CoffeeShopDbContext context)
		{
			_context = context;
		}

		public async Task Handle(DeleteCoffeeCommand request, CancellationToken cancellationToken)
		{
			_context.Coffees.Remove(request.Coffee);
			await _context.SaveChangesAsync();
		}
	}
}
