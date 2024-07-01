using CoffeeShopMinimalApi.Infrastructure.Context;
using CoffeeShopMinimalApi.Infrastructure.Modules.Coffees.Queries;
using CoffeeShopMinimalApi.Modules.Coffees.Endpoints;
using CoffeeShopMinimalApi.Modules.Coffees.Models;
using CoffeeShopMinimalApi.Modules.Ingredients.Models;
using CoffeeShopMinimalApi.Modules.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CoffeeShopMinimalApi.Tests.IntegrationTests
{
	internal class CoffeeEndpointTests
	{
		private ICoffeeEndpoint _coffeeEndpoint;

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			var services = new ServiceCollection();
			var mockContextDbContext = new Mock<CoffeeShopDbContext>();

			services.AddDbContext<CoffeeShopDbContext>(opt => opt.UseInMemoryDatabase("CoffeeDB"));

			var serviceProvider = services
				.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetCoffeesQueryHandler).Assembly))
				.BuildServiceProvider();
			var mediator = serviceProvider.GetRequiredService<IMediator>();

			_coffeeEndpoint = new CoffeeEndpoint(mediator);
		}

		[Test]
		public async Task CreateCoffeeAddItemToCollection()
		{
			// Arrange
			var coffeDto = new CoffeeDto
			{
				Name = "Test Coffee",
				Price = 10,
				Ingredients = new List<IngredientDto>
				{
					new IngredientDto { Name = "Test Ingredient" }
				}
			};

			// Act
			var createResponse = await _coffeeEndpoint.Create(coffeDto);

			// Assert
			Assert.IsNotNull(createResponse);
			Assert.IsInstanceOf<Created<CoffeeDto>>(createResponse);

			var createResult = (Created<CoffeeDto>)createResponse;
			Assert.NotNull(createResult.Value);

			// Act
			var response = await _coffeeEndpoint.GetById(createResult.Value.Id);

			//Assert
			Assert.NotNull(response);
			Assert.IsInstanceOf<Ok<CoffeeDto>>(response);

			var result = (Ok<CoffeeDto>)response;
			Assert.NotNull(result);
			Assert.NotNull(result.Value);
			Assert.That(createResult.Value.Id, Is.EqualTo(result.Value.Id));
			Assert.That(result.Value.Name, Is.EqualTo(coffeDto.Name));
		}

		[Test]
		public async Task DeleteCoffeeRemoveItemFromCollection()
		{
			// Arrange
			var coffeDto = new CoffeeDto
			{
				Name = "Test Coffee",
				Price = 10,
				Ingredients = new List<IngredientDto>
				{
					new IngredientDto { Name = "Test Ingredient" }
				}
			};

			// Act
			var createResponse = await _coffeeEndpoint.Create(coffeDto);

			// Assert
			Assert.IsNotNull(createResponse);
			Assert.IsInstanceOf<Created<CoffeeDto>>(createResponse);

			var createResult = (Created<CoffeeDto>)createResponse;
			Assert.NotNull(createResult.Value);

			// Act
			var deleteResponse = await _coffeeEndpoint.Delete(createResult.Value.Id);

			// Assert
			Assert.NotNull(deleteResponse);
			Assert.IsInstanceOf<NoContent>(deleteResponse);

			// Act
			var response = await _coffeeEndpoint.GetById(createResult.Value.Id);

			//Assert
			Assert.NotNull(response);
			Assert.IsInstanceOf<NotFound>(response);
		}
	}
}
