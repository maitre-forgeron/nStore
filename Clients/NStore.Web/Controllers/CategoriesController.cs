using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NStore.Web.Models.Products;
using NStore.Web.Services.Interfaces;

namespace NStore.Web.Controllers;

[Authorize]
public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _categoryService.GetCategoriesAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        return View(await _categoryService.GetCategoryAsync(id));
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryViewModel categoryViewModel)
    {
        if (ModelState.IsValid)
        {
            await _categoryService.AddCategoryAsync(categoryViewModel);

            return RedirectToAction(nameof(Index));
        }

        return View(categoryViewModel);
    }

    public async Task<IActionResult> Edit(int id)
    {
        return View(await _categoryService.GetCategoryAsync(id));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CategoryViewModel categoryViewModel)
    {
        if (ModelState.IsValid)
        {
            await _categoryService.UpdateCategoryAsync(categoryViewModel);

            return RedirectToAction(nameof(Index));
        }
        return View(categoryViewModel);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var category = await _categoryService.GetCategoryAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        await _categoryService.DeleteCategoryWithRelatedProductsAsync(id);

        return RedirectToAction(nameof(Index));
    }
}
