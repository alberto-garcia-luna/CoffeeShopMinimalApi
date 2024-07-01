using CoffeeShopMinimalApi.Infrastructure.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CoffeeShopMinimalApi.Infrastructure.Extensions
{
	public static class Configuration
	{
		public static void RegisterInfrastructureServices(this WebApplicationBuilder builder)
		{
			builder.Services.AddDbContext<CoffeeShopDbContext>(opt =>
				opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
				b => b.MigrationsAssembly("CoffeeShopMinimalApi.Infrastructure"))
			);

			builder.Services.AddDatabaseDeveloperPageExceptionFilter();
			builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
		}

		public static void RegisterInfrastructureMiddlewares(this WebApplication app)
		{
			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;

				var context = services.GetRequiredService<CoffeeShopDbContext>();				
				if (context.Database.GetPendingMigrations().Any())
				{
					context.Database.EnsureCreated();
				}
			}
		}
	}
}