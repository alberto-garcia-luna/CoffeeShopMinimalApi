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
		private Mock<CoffeeShopDbContext> _mockContextDbContext;
		private Guid _validCoffeeId;

		[SetUp]
		public void Setup()
		{
			_coffees = TestDataGenerator.GetTestCoffees();
			_validCoffeeId = _coffees.FirstOrDefault().Id;

			var services = new ServiceCollection();
			_mockContextDbContext = new Mock<CoffeeShopDbContext>();
			
			_mockContextDbContext.Setup(c => c.Coffees).ReturnsDbSet(_coffees);
			_mockContextDbContext.Setup(c => c.Coffees.Add(It.IsAny<Coffee>())).Callback(_coffees.Add);
			_mockContextDbContext.Setup(c => c.Coffees.Remove(It.IsAny<Coffee>())).Callback<Coffee>((item) => _coffees.Remove(item));
			_mockContextDbContext.Setup(i => i.Coffees.Update(It.IsAny<Coffee>())).Callback<Coffee>(
				(item) => {
					_coffees.Remove(item);
					_coffees.Add(item);
				});

			services.AddTransient(opt => { return _mockContextDbContext.Object; });

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
		public async Task GetAllCoffeesReturnsValuesIfExists()
		{
			// Arrange
			// Act
			var response = await _coffeeEndpoint.GetAll();

			//Assert
			Assert.NotNull(response);
			Assert.IsInstanceOf<Ok<IEnumerable<CoffeeDto>>>(response);

			var result = (Ok<IEnumerable<CoffeeDto>>)response;
			Assert.NotNull(result.Value);
			Assert.That(result.Value.Count, Is.EqualTo(_mockContextDbContext.Object.Coffees.Count()));
		}

		[Test]
		public async Task GetCoffeeIngredientsReturnsValuesIfExists()
		{
			// Arrange
			var ingredientsList = _mockContextDbContext.Object.Coffees
				.FirstOrDefault(item => item.Id.Equals(_validCoffeeId))?
				.Ingredients;

			// Act
			var response = await _coffeeEndpoint.GetCoffeeIngredients(_validCoffeeId);

			// Assert
			Assert.NotNull(response);
			Assert.IsInstanceOf<Ok<IEnumerable<IngredientDto>>>(response);
			
			var result = (Ok<IEnumerable<IngredientDto>>)response;
			Assert.NotNull(result.Value);
			Assert.That(result.Value.Count, Is.EqualTo(ingredientsList?.Count()));
		}

		[Test]
		public async Task CreateCoffeeAddItemToCollection()
		{
			// Arrange
			var coffeesListSize = _mockContextDbContext.Object.Coffees.Count();
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
			Assert.That(_mockContextDbContext.Object.Coffees.Count, Is.EqualTo(coffeesListSize + 1));
			Assert.IsTrue(_mockContextDbContext.Object.Coffees.ToList().Exists(item => item.Name.Equals(coffeDto.Name)));
		}

		[Test]
		public async Task UpdateCoffeeReturnsNotFoundIfNotExists()
		{
			// Arrange
			var invalidGuid = Guid.Empty;
			var coffeDto = new CoffeeDto
			{
				Id = invalidGuid,
				Name = "Test Coffee",
				Price = 10,
				Ingredients = new List<IngredientDto>
				{
					new IngredientDto { Name = "Test Ingredient" }
				}
			};

			// Act
			var response = await _coffeeEndpoint.Update(invalidGuid, coffeDto);

			//Assert
			Assert.NotNull(response);
			Assert.IsInstanceOf<NotFound>(response);
		}

		[Test]
		public async Task UpdateCoffeeUpdateItemFromCollection()
		{
			// Arrange
			var coffeDto = new CoffeeDto
			{
				Id = _validCoffeeId,
				Name = "Test Coffee",
				Price = 10,
				Ingredients = new List<IngredientDto>
				{
					new IngredientDto { Name = "Test Ingredient" }
				}
			};

			// Act
			var response = await _coffeeEndpoint.Update(_validCoffeeId, coffeDto);

			//Assert
			Assert.NotNull(response);
			Assert.IsInstanceOf<NoContent>(response);

			var coffeeInList = 
				_mockContextDbContext.Object.Coffees.FirstOrDefault(item => item.Id.Equals(_validCoffeeId));
			Assert.NotNull(coffeeInList);
			Assert.That(coffeeInList.Name, Is.EqualTo(coffeDto.Name));
		}

		[Test]
		public async Task DeleteCoffeeReturnsNotFoundIfNotExists()
		{
			// Arrange
			var invalidGuid = Guid.Empty;

			// Act
			var response = await _coffeeEndpoint.Delete(invalidGuid);

			//Assert
			Assert.NotNull(response);
			Assert.IsInstanceOf<NotFound>(response);
		}

		[Test]
		public async Task DeleteCoffeeRemoveItemFromCollection()
		{
			// Arrange
			var coffeesListSize = _mockContextDbContext.Object.Coffees.Count();

			// Act
			var response = await _coffeeEndpoint.Delete(_validCoffeeId);

			// Assert
			Assert.NotNull(response);
			Assert.IsInstanceOf<NoContent>(response);

			Assert.That(_mockContextDbContext.Object.Coffees.Count, Is.EqualTo(coffeesListSize - 1));
		}
	}
}