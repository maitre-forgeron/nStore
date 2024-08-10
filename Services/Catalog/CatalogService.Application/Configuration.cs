using CatalogService.Application.Mappers;
using CatalogService.Application.Products.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CatalogService.Application
{
    public static class Configuration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(AddProductCommand))));
            services.AddScoped<IProductsMapper, ProductsMapper>();

            return services;
        }
    }
}
