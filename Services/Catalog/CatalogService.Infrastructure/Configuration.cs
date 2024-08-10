using CatalogService.Domain.Interfaces.Repositories;
using CatalogService.Infrastructure.Db;
using CatalogService.Infrastructure.Db.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Infrastructure
{
    public static class Configuration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CatalogServiceDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CatalogServiceDbContext"), builder =>
                {
                    builder.EnableRetryOnFailure();
                });
            });
            services.AddScoped<CatalogServicesDbContextInitialiser>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
