using AutoMapper;
using CoffeeShopMinimalApi.Extensions;
using CoffeeShopMinimalApi.Infrastructure.Entities;
using CoffeeShopMinimalApi.Infrastructure.Modules.Coffees.Commands;
using CoffeeShopMinimalApi.Infrastructure.Modules.Ingredients.Commands;
using CoffeeShopMinimalApi.Infrastructure.Modules.Ingredients.Queries;
using CoffeeShopMinimalApi.Modules.Ingredients.Models;
using CoffeeShopMinimalApi.Modules.Interfaces;
using MediatR;

namespace CoffeeShopMinimalApi.Modules.Ingredients.Endpoints
{
	public class IngredientEndpoint : IIngredientEndpoint
	{
		private readonly IMediator _mediator;
		private readonly Mapper _mapper;

		public IngredientEndpoint(IMediator mediator)
		{
			_mediator = mediator;
			_mapper = AutoMapperCongiguration.InitAutoMapper();
		}

		public async Task<IResult> GetAll()
		{
			var ingredients = await _mediator.Send(new GetIngredientsQuery());

			return TypedResults.Ok(_mapper.Map<IEnumerable<IngredientDto>>(ingredients.Ingredients));
		}

		public async Task<IResult> GetById(Guid id)
		{
			var query = new GetIngredientQuery()
			{
				Id = id
			};

			var response = await _mediator.Send(query);

			return response?.Ingredient == null
				? TypedResults.NotFound()
				: TypedResults.Ok(_mapper.Map<IngredientDto>(response.Ingredient));
		}

		public async Task<IResult> Create(IngredientDto ingredientDto)
		{
			var ingredient = _mapper.Map<Ingredient>(ingredientDto);
			var command = new CreateIngredientCommand()
			{
				Ingredient = ingredient
			};
			var response = await _mediator.Send(command);
			ingredientDto = _mapper.Map<IngredientDto>(response.Ingredient);

			return TypedResults.Created($"{Constants.ModulePath}/{ingredientDto.Id}", ingredientDto);
		}

		public async Task<IResult> Update(Guid id, IngredientDto ingredientDto)
		{
			var query = new GetIngredientQuery()
			{
				Id = id
			};

			var response = await _mediator.Send(query);
			if (response.Ingredient is null)
				return TypedResults.NotFound();

			response.Ingredient.Name = ingredientDto.Name;

			var command = new UpdateIngredientCommand()
			{
				Ingredient = response.Ingredient
			};

			await _mediator.Send(command);
			return TypedResults.NoContent();
		}

		public async Task<IResult> Delete(Guid id)
		{
			var query = new GetIngredientQuery()
			{
				Id = id
			};

			var response = await _mediator.Send(query);
			if (response.Ingredient is null)
				return TypedResults.NotFound();

			var command = new DeleteIngredientCommand()
			{
				Ingredient = response.Ingredient
			};

			await _mediator.Send(command);
			return TypedResults.NoContent();
		}
	}
}
