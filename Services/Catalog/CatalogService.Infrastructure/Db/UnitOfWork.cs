using CatalogService.Domain.Interfaces.Repositories;

namespace CatalogService.Infrastructure.Db
{
    public class UnitOfWork : IUnitOfWork
    {
        public IProductRepository ProductRepository { get; init; }
        public ICategoryRepository CategoryRepository { get; init; }

        private readonly CatalogServiceDbContext _context;

        public UnitOfWork(IProductRepository productRepository, ICategoryRepository categoryRepository, CatalogServiceDbContext context)
        {
            ProductRepository = productRepository;
            CategoryRepository = categoryRepository;

            _context = context;
        }

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();
    }
}
