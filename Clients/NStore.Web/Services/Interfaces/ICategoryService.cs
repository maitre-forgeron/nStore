using NStore.Web.Models.Products;

namespace NStore.Web.Services.Interfaces;

public interface ICategoryService
{
    Task<CategoryViewModel> GetCategoryAsync(int id);

    Task<List<CategoryViewModel>> GetCategoriesAsync();

    Task<int> AddCategoryAsync(CategoryViewModel model);

    Task UpdateCategoryAsync(CategoryViewModel model);

    Task<bool> DeleteCategoryWithRelatedProductsAsync(int id);
}
