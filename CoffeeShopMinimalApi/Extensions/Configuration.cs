using Microsoft.OpenApi.Models;

namespace CoffeeShopMinimalApi.Extensions
{
	public static class Configuration
	{
        public static void RegisterServices(this WebApplicationBuilder builder)
        {
			builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Coffee API", Version = "v1" });
            });
        }

        public static void RegisterMiddlewares(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
        }
    }
}
