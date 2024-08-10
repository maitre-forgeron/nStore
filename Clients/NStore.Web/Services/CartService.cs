using NStore.Web.Models.Carts;
using NStore.Web.Services.Base;
using NStore.Web.Services.Interfaces;

namespace NStore.Web.Services;

public class CartService : ICartService
{
    private readonly HttpClientService _http;

    public CartService(HttpClientService http)
    {
        _http = http;
    }

    public async Task<CartViewModel> GetItemsV1Async(string cartId) => await _http.GetAsync<CartViewModel>($"carting-service/cart/{cartId}");

    public async Task<ItemViewModel> AddItemToCartAsync(object model) => await _http.PostAsync<ItemViewModel>("carting-service/cart", model);

    public async Task<bool> RemoveItemFromCart(string cartId, int itemId) => await _http.DeleteAsync($"carting-service/cart/{cartId}/{itemId}");
}
