using NStore.Web.Models.Carts;

namespace NStore.Web.Services.Interfaces;

public interface ICartService
{
    Task<CartViewModel> GetItemsV1Async(string cartId);

    Task<ItemViewModel> AddItemToCartAsync(object model);

    Task<bool> RemoveItemFromCart(string cartId, int itemId);
}
