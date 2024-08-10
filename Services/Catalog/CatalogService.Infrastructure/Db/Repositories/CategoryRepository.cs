using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces.Repositories;

namespace CatalogService.Infrastructure.Db.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(CatalogServiceDbContext context) : base(context)
        {
        }
    }
}
