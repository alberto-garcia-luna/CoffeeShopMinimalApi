using AutoMapper;
using CoffeeShopMinimalApi.Infrastructure.Entities;
using CoffeeShopMinimalApi.Modules.Ingredients.Models;
using CoffeeShopMinimalApi.Modules.Coffees.Models;

namespace CoffeeShopMinimalApi.Extensions
{
	public static class AutoMapperCongiguration
	{
		public static Mapper InitAutoMapper()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<Coffee, CoffeeDto>();
				cfg.CreateMap<CoffeeDto, Coffee>();
				cfg.CreateMap<Ingredient, IngredientDto>();
				cfg.CreateMap<IngredientDto, Ingredient>();
			});

			var mapper = new Mapper(config);
			return mapper;
		}
	}
}
