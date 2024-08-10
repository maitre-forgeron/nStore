using NStore.Web.Models.Products;

namespace NStore.Web.Services.Interfaces;

public interface IProductService
{
    Task<ProductViewModel> GetProductAsync(int id);

    Task<ProductListViewModel> GetProductsAsync(string? previousPageUrl = null, string? nextPageUrl = null);

    Task<ProductViewModel> AddProductAsync(AddProductViewModel product);

    Task UpdateProductAsync(UpdateProductViewModel product);

    Task<bool> DeleteProductAsync(int id);
}
