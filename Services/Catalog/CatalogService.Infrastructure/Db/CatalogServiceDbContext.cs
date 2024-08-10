using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CatalogService.Infrastructure.Db
{
    public class CatalogServiceDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public CatalogServiceDbContext(DbContextOptions<CatalogServiceDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
