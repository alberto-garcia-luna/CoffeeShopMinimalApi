using CoffeeShopMinimalApi.Infrastructure.Context;
using CoffeeShopMinimalApi.Infrastructure.Entities;
using CoffeeShopMinimalApi.Infrastructure.Modules.Coffees.Queries;
using CoffeeShopMinimalApi.Modules.Ingredients.Endpoints;
using CoffeeShopMinimalApi.Modules.Ingredients.Models;
using CoffeeShopMinimalApi.Modules.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.EntityFrameworkCore;

namespace CoffeeShopMinimalApi.Tests.UnitTests
{
	internal class IngredientEndpointTests
	{
		private IIngredientEndpoint _ingredientEndpoint;
		private IList<Ingredient> _ingredients;
		private Mock<CoffeeShopDbContext> _mockContextDbContext;
		private Guid _validIngredientId;

		[SetUp]
		public void Setup()
		{
			_ingredients = TestDataGenerator.GetTestIngredients();
			_validIngredientId = _ingredients.FirstOrDefault().Id;

			var services = new ServiceCollection();
			_mockContextDbContext = new Mock<CoffeeShopDbContext>();

			_mockContextDbContext.Setup(i => i.Ingredients).ReturnsDbSet(_ingredients);
			_mockContextDbContext.Setup(i => i.Ingredients.Add(It.IsAny<Ingredient>())).Callback(_ingredients.Add);
			_mockContextDbContext.Setup(i => i.Ingredients.Remove(It.IsAny<Ingredient>())).Callback<Ingredient>((item) => _ingredients.Remove(item));
			_mockContextDbContext.Setup(i => i.Ingredients.Update(It.IsAny<Ingredient>())).Callback<Ingredient>(
				(item) => {
					_ingredients.Remove(item);
					_ingredients.Add(item);
				});

			services.AddTransient(opt => { return _mockContextDbContext.Object; });

			var serviceProvider = services
				.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetCoffeesQueryHandler).Assembly))
				.BuildServiceProvider();
			var mediator = serviceProvider.GetRequiredService<IMediator>();

			_ingredientEndpoint = new IngredientEndpoint(mediator);
		}

		[Test]
		public async Task GetIngredientReturnsNotFoundIfNotExists()
		{
			// Arrange
			var invalidGuid = Guid.Empty;

			// Act
			var response = await _ingredientEndpoint.GetById(invalidGuid);

			//Assert
			Assert.NotNull(response);
			Assert.IsInstanceOf<NotFound>(response);
		}

		[Test]
		public async Task GetIngredientReturnsValuesIfExists()
		{
			// Arrange
			// Act
			var response = await _ingredientEndpoint.GetById(_validIngredientId);

			//Assert
			Assert.NotNull(response);
			Assert.IsInstanceOf<Ok<IngredientDto>>(response);

			var result = (Ok<IngredientDto>)response;
			Assert.NotNull(result.Value);
			Assert.That(result.Value.Id, Is.EqualTo(_validIngredientId));
		}

		[Test]
		public async Task GetAllIngredientsReturnsValuesIfExists()
		{
			// Arrange
			// Act
			var response = await _ingredientEndpoint.GetAll();

			//Assert
			Assert.NotNull(response);
			Assert.IsInstanceOf<Ok<IEnumerable<IngredientDto>>>(response);

			var result = (Ok<IEnumerable<IngredientDto>>)response;
			Assert.NotNull(result.Value);
			Assert.That(result.Value.Count, Is.EqualTo(_mockContextDbContext.Object.Ingredients.Count()));
		}

		[Test]
		public async Task CreateIngredientAddItemToCollection()
		{
			// Arrange
			var ingredientsListSize = _mockContextDbContext.Object.Ingredients.Count();
			var ingredientDto = new IngredientDto
			{
				Name = "Test Ingredient"
			};

			// Act
			var response = await _ingredientEndpoint.Create(ingredientDto);

			// Assert
			Assert.NotNull(response);
			Assert.IsInstanceOf<Created<IngredientDto>>(response);

			var result = (Created<IngredientDto>)response;

			Assert.NotNull(result.Value);
			Assert.That(_mockContextDbContext.Object.Ingredients.Count, Is.EqualTo(ingredientsListSize + 1));
			Assert.IsTrue(_mockContextDbContext.Object.Ingredients.ToList().Exists(item => item.Name.Equals(ingredientDto.Name)));
		}

		[Test]
		public async Task UpdateIngredientReturnsNotFoundIfNotExists()
		{
			// Arrange
			var invalidGuid = Guid.Empty;
			var ingredientDto = new IngredientDto
			{
				Id = invalidGuid,
				Name = "Test Coffee"
			};

			// Act
			var response = await _ingredientEndpoint.Update(invalidGuid, ingredientDto);

			//Assert
			Assert.NotNull(response);
			Assert.IsInstanceOf<NotFound>(response);
		}

		[Test]
		public async Task UpdateIngredientUpdateItemFromCollection()
		{
			// Arrange
			var ingredientDto = new IngredientDto
			{
				Id = _validIngredientId,
				Name = "Test Coffee"
			};

			// Act
			var response = await _ingredientEndpoint.Update(_validIngredientId, ingredientDto);

			//Assert
			Assert.NotNull(response);
			Assert.IsInstanceOf<NoContent>(response);

			var coffeeInList =
				_mockContextDbContext.Object.Ingredients.FirstOrDefault(item => item.Id.Equals(_validIngredientId));
			Assert.NotNull(coffeeInList);
			Assert.That(coffeeInList.Name, Is.EqualTo(ingredientDto.Name));
		}

		[Test]
		public async Task DeleteIngredientReturnsNotFoundIfNotExists()
		{
			// Arrange
			var invalidGuid = Guid.Empty;

			// Act
			var response = await _ingredientEndpoint.Delete(invalidGuid);

			//Assert
			Assert.NotNull(response);
			Assert.IsInstanceOf<NotFound>(response);
		}

		[Test]
		public async Task DeleteIngredientRemoveItemFromCollection()
		{
			// Arrange
			var ingredientsListSize = _mockContextDbContext.Object.Ingredients.Count();

			// Act
			var response = await _ingredientEndpoint.Delete(_validIngredientId);

			// Assert
			Assert.NotNull(response);
			Assert.IsInstanceOf<NoContent>(response);

			Assert.That(_mockContextDbContext.Object.Ingredients.Count, Is.EqualTo(ingredientsListSize - 1));
		}
	}
}
