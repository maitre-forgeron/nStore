using NStore.Web.Models.Products;
using NStore.Web.Services.Base;
using NStore.Web.Services.Interfaces;

namespace NStore.Web.Services;

public class CategoryService : ICategoryService
{
    private readonly HttpClientService _http;

    public CategoryService(HttpClientService http)
    {
        _http = http;
    }

    public async Task<CategoryViewModel> GetCategoryAsync(int id) => await _http.GetAsync<CategoryViewModel>($"catalog-service/category/{id}");

    public async Task<List<CategoryViewModel>> GetCategoriesAsync() => await _http.GetAsync<List<CategoryViewModel>>("catalog-service/category");

    public async Task<int> AddCategoryAsync(CategoryViewModel model) => await _http.PostAsync<int>("catalog-service/category", model);

    public async Task UpdateCategoryAsync(CategoryViewModel model) => await _http.PutAsync<object>("catalog-service/category", model);

    public async Task<bool> DeleteCategoryWithRelatedProductsAsync(int id) => await _http.DeleteAsync($"catalog-service/category/{id}");
}
