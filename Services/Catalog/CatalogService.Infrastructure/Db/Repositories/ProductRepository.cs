using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces.Repositories;

namespace CatalogService.Infrastructure.Db.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(CatalogServiceDbContext context) : base(context)
        {
        }
    }
}
