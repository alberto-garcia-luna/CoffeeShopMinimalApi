using AutoMapper;
using CoffeeShopMinimalApi.Extensions;
using CoffeeShopMinimalApi.Infrastructure.Entities;
using CoffeeShopMinimalApi.Infrastructure.Modules.Coffees.Commands;
using CoffeeShopMinimalApi.Infrastructure.Modules.Coffees.Queries;
using CoffeeShopMinimalApi.Modules.Coffees.Models;
using CoffeeShopMinimalApi.Modules.Ingredients.Models;
using CoffeeShopMinimalApi.Modules.Interfaces;
using MediatR;

namespace CoffeeShopMinimalApi.Modules.Coffees.Endpoints
{
	public class CoffeeEndpoint : ICoffeeEndpoint
	{
		private readonly IMediator _mediator;
		private readonly Mapper _mapper;

		public CoffeeEndpoint(IMediator mediator)
		{
			_mediator = mediator;
			_mapper = AutoMapperCongiguration.InitAutoMapper();
		}

		public async Task<IResult> GetAll()
		{
			var response = await _mediator.Send(new GetCoffeesQuery());

			return TypedResults.Ok(_mapper.Map<IEnumerable<CoffeeDto>>(response.Coffees));
		}

		public async Task<IResult> GetById(Guid id)
		{
			var query = new GetCoffeeQuery()
			{
				Id = id
			};

			var response = await _mediator.Send(query);

			return response?.Coffee == null
				? TypedResults.NotFound()
				: TypedResults.Ok(_mapper.Map<CoffeeDto>(response.Coffee));
		}

		public async Task<IResult> GetCoffeeIngredients(Guid id)
		{
			var query = new GetCoffeeQuery()
			{
				Id = id
			};

			var response = await _mediator.Send(query);

			return response.Coffee == null
				? TypedResults.NotFound()
				: TypedResults.Ok(_mapper.Map<IEnumerable<IngredientDto>>(response.Coffee.Ingredients));
		}

		public async Task<IResult> Create(CoffeeDto coffeeDto)
        {
            var coffee = _mapper.Map<Coffee>(coffeeDto);
			var command = new CreateCoffeeCommand()
			{
				Coffee = coffee
			};

			var response = await _mediator.Send(command);
            coffeeDto = _mapper.Map<CoffeeDto>(response.Coffee);

            return TypedResults.Created($"{Constants.ModulePath}/{coffeeDto.Id}", coffeeDto);
        }

        public async Task<IResult> Update(Guid id, CoffeeDto coffeeDto)
        {
			var query = new GetCoffeeQuery()
			{
				Id = id
			};

			var response = await _mediator.Send(query);
			if (response.Coffee is null) 
				return TypedResults.NotFound();

			response.Coffee.Name = coffeeDto.Name;
			response.Coffee.Price = coffeeDto.Price;
			response.Coffee.Ingredients = _mapper.Map<IEnumerable<Ingredient>>(coffeeDto.Ingredients);

			var command = new UpdateCoffeeCommand()
			{
				Coffee = response.Coffee
			};

			await _mediator.Send(command);
			return TypedResults.NoContent();
        }

        public async Task<IResult> Delete(Guid id)
        {
			var query = new GetCoffeeQuery()
			{
				Id = id
			};

			var response = await _mediator.Send(query);
			if (response.Coffee is null)
				return TypedResults.NotFound();

			var command = new DeleteCoffeeCommand()
			{
				Coffee = response.Coffee
			};

			await _mediator.Send(command);
			return TypedResults.NoContent();
		}
    }
}
