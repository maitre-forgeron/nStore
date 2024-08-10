using NStore.Web.Models.Products;
using NStore.Web.Models.Shared;
using NStore.Web.Services.Base;
using NStore.Web.Services.Interfaces;

namespace NStore.Web.Services;

public class ProductService : IProductService
{
    private readonly HttpClientService _http;

    public ProductService(HttpClientService http)
    {
        _http = http;
    }

    public async Task<ProductViewModel> GetProductAsync(int id) => await _http.GetAsync<ProductViewModel>($"catalog-service/products/{id}");

    public async Task<ProductListViewModel> GetProductsAsync(string? previousPageUrl = null, string? nextPageUrl = null)
    {
        (List<ProductViewModel> Result, PaginationViewModel HeaderData) result = (new(), new());

        if (string.IsNullOrWhiteSpace(nextPageUrl) && string.IsNullOrWhiteSpace(previousPageUrl))
            result = await _http.GetAsync<List<ProductViewModel>, PaginationViewModel>($"catalog-service/products", "X-Pagination");

        if (!string.IsNullOrWhiteSpace(nextPageUrl))
            result = await _http.GetAsync<List<ProductViewModel>, PaginationViewModel>($"catalog-service/products?{nextPageUrl}", "X-Pagination");

        if(!string.IsNullOrWhiteSpace(previousPageUrl))
            result = await _http.GetAsync<List<ProductViewModel>, PaginationViewModel>($"catalog-service/products?{previousPageUrl}", "X-Pagination");

        return new ProductListViewModel { Products = result.Result, Pagination = result.HeaderData };
    }

    public async Task<ProductViewModel> AddProductAsync(AddProductViewModel product) => await _http.PostAsync<ProductViewModel>("catalog-service/products", product);

    public async Task UpdateProductAsync(UpdateProductViewModel product) => await _http.PutAsync<object>("catalog-service/products", product);
    
    public async Task<bool> DeleteProductAsync(int id) => await _http.DeleteAsync($"catalog-service/products/{id}");
}
