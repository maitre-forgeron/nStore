using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CatalogService.Infrastructure.Db
{
    public class CatalogServiceDbContextFactory : IDesignTimeDbContextFactory<CatalogServiceDbContext>
    {
        public CatalogServiceDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .Build();

            var connectionString = config.GetConnectionString("CatalogServiceDbContext");

            var optionsBuilder = new DbContextOptionsBuilder<CatalogServiceDbContext>()
                .UseSqlServer(connectionString);

            return new CatalogServiceDbContext(optionsBuilder.Options);
        }
    }
}
