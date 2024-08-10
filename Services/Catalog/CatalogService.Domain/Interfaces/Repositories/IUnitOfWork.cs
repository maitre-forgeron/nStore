namespace CatalogService.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }

        ICategoryRepository CategoryRepository { get; }

        Task<int> CommitAsync();
    }
}
