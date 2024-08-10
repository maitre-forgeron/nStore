using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NStore.Web.Extensions;
using NStore.Web.Models.Products;
using NStore.Web.Services.Interfaces;

namespace NStore.Web.Controllers;

[Authorize]
public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductsController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index([FromQuery] string previousPageUrl, [FromQuery] string nextPageUrl, [FromQuery] bool isCurrentPage = true)
    {
        ProductListViewModel products = isCurrentPage ?
            await _productService.GetProductsAsync() :
            await _productService.GetProductsAsync(previousPageUrl, nextPageUrl);

        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _productService.GetProductAsync(id);

        return View(product);
    }

    public IActionResult AddToCart(int id)
    {
        var userId = User.GetAuthenticatedUserId();

        return RedirectToAction("Create", "Carts", new { cartId = userId, itemId = id });
    }

    public async Task<IActionResult> Create()
    {
        var model = new AddProductViewModel(await _categoryService.GetCategoriesAsync());

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AddProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            await _productService.AddProductAsync(model);

            return RedirectToAction(nameof(Index));
        }

        return View("Error");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productService.GetProductAsync(id);

        var model = new UpdateProductViewModel(product, await _categoryService.GetCategoriesAsync());

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            await _productService.UpdateProductAsync(model);

            return RedirectToAction(nameof(Index));
        }

        return View("Error");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productService.GetProductAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        await _productService.DeleteProductAsync(id);

        return RedirectToAction(nameof(Index));
    }
}
