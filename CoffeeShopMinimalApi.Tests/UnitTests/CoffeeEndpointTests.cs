using CoffeeShopMinimalApi.Infrastructure.Context;
using CoffeeShopMinimalApi.Infrastructure.Entities;
using CoffeeShopMinimalApi.Infrastructure.Modules.Coffees.Queries;
using CoffeeShopMinimalApi.Modules.Coffees.Endpoints;
using CoffeeShopMinimalApi.Modules.Coffees.Models;
using CoffeeShopMinimalApi.Modules.Ingredients.Models;
using CoffeeShopMinimalApi.Modules.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.EntityFrameworkCore;

namespace CoffeeShopMinimalApi.Tests.UnitTests
{
	internal class CoffeeEndpointTests
	{
		private ICoffeeEndpoint _coffeeEndpoint;
		private IList<Coffee> _coffees;
		private IList<Ingredient> _ingredients;
		private Guid _validCoffeeId;

		[SetUp]
		public void Setup()
		{
			GetTestData();
			var services = new ServiceCollection();
			var mockContext = new Mock<CoffeeShopDbContext>();

			mockContext.Setup(i => i.Ingredients).ReturnsDbSet(_ingredients);
			mockContext.Setup(i => i.Ingredients.Add(It.IsAny<Ingredient>())).Callback(_ingredients.Add);
			mockContext.Setup(c => c.Coffees).ReturnsDbSet(_coffees);
			mockContext.Setup(c => c.Coffees.Add(It.IsAny<Coffee>())).Callback(_coffees.Add);

			services.AddTransient(opt => { return mockContext.Object; });

			var serviceProvider = services
				.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetCoffeesQueryHandler).Assembly))
				.BuildServiceProvider();
			var mediator = serviceProvider.GetRequiredService<IMediator>();

			_coffeeEndpoint = new CoffeeEndpoint(mediator);
		}

		[Test]
		public async Task GetCoffeeReturnsNotFoundIfNotExists()
		{
			// Arrange
			var invalidGuid = Guid.Empty;

			// Act
			var response = await _coffeeEndpoint.GetById(invalidGuid);

			//Assert
			Assert.NotNull(response);
			Assert.IsInstanceOf<NotFound>(response);
		}

		[Test]
		public async Task GetCoffeeReturnsValuesIfExists()
		{
			// Arrange
			// Act
			var response = await _coffeeEndpoint.GetById(_validCoffeeId);

			//Assert
			Assert.NotNull(response);
			Assert.IsInstanceOf<Ok<CoffeeDto>>(response);

			var result = (Ok<CoffeeDto>)response;
			Assert.NotNull(result.Value);
			Assert.That(result.Value.Id, Is.EqualTo(_validCoffeeId));
		}

		[Test]
		public async Task CreateCoffeeAddItemToCollection()
		{
			// Arrange
			var coffeeListSize = _coffees.ToList().Count;
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
			var response = await _coffeeEndpoint.Create(coffeDto);

			// Assert
			Assert.NotNull(response);
			Assert.IsInstanceOf<Created<CoffeeDto>>(response);

			var result = (Created<CoffeeDto>)response;

			Assert.NotNull(result.Value);
			Assert.That(_coffees.ToList().Count, Is.EqualTo(coffeeListSize + 1));
			Assert.IsTrue(_coffees.ToList().Exists(item => item.Name.Equals(coffeDto.Name)));
		}

		private void GetTestData()
		{
			_validCoffeeId = Guid.NewGuid();
			_ingredients =
			[
				new Ingredient()
				{
					Id = Guid.NewGuid(),
					Name = "Roasted Coffee"
				},
				new Ingredient()
				{
					Id = Guid.NewGuid(),
					Name = "Milk"
				},
				new Ingredient()
				{
					Id = Guid.NewGuid(),
					Name = "Water"
				},
			];

			_coffees =
			[
				new Coffee()
				{
					Id = _validCoffeeId,
					Name = "Cappuccino",
					Price = 50,
					Ingredients = _ingredients
				}
			];
		}
	}
}