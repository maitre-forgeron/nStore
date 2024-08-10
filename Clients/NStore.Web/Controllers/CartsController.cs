using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NStore.Web.Extensions;
using NStore.Web.Models.Carts;
using NStore.Web.Services.Interfaces;

namespace NStore.Web.Controllers;

[Authorize]
public class CartsController : Controller
{
    private readonly ICartService _cartService;
    private readonly IProductService _productService;

    public CartsController(ICartService cartService, IProductService productService)
    {
        _cartService = cartService;
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.GetAuthenticatedUserId();

        var cart = await _cartService.GetItemsV1Async(userId);

        return View(cart);
    }

    [HttpGet]
    public async Task<IActionResult> Create(string cartId, int itemId)
    {
        var product = await _productService.GetProductAsync(itemId);

        if (product == null)
        {
            return View("Error");
        }

        var item = new AddItemViewModel(product);

        var cart = new { Oid = cartId, Item = item };

        await _cartService.AddItemToCartAsync(cart);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.GetAuthenticatedUserId();

        await _cartService.RemoveItemFromCart(userId, id);

        return RedirectToAction(nameof(Index));
    }
}
