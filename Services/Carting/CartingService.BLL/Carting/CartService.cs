using NStore.Shared.Exceptions;

namespace CartingService.BLL.Carting
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<bool> AddItemAsync(string oId, Item item)
        {
            await EnsureItemCreated(item);

            bool success;

            var cart = await _cartRepository.GetCartAsync(oId);

            if (cart == null)
            {
                cart = new Cart(oId, new List<int> { item.Id });

                success = !string.IsNullOrWhiteSpace(await _cartRepository.InsertCartAsync(cart));
            }
            else
            {
                cart.AddItem(item.Id);

                success = await _cartRepository.UpdateCartAsync(cart);
            }

            return success;
        }

        public async Task<bool> UpdateItemAsync(Item item) => await _cartRepository.UpdateItemAsync(item);

        public async Task<(Cart Cart, List<Item> Items)> GetCartAsync(string oId)
        {
            var cart = await _cartRepository.GetCartAsync(oId);

            if (cart == null)
            {
                return (null, []);
            }

            var items = await _cartRepository.GetItemsAsync(cart.Items);

            return (cart, items);
        }

        public async Task<int> RemoveItemAsync(string oId, int itemId)
        {
            var cart = await _cartRepository.GetCartAsync(oId);

            cart.RemoveItem(itemId);

            await _cartRepository.UpdateCartAsync(cart);

            return itemId;
        }

        public async Task<Item> GetItemAsync(int id) => await _cartRepository.GetItemAsync(id);

        private async Task<bool> ItemExistsAsync(int id) => await _cartRepository.GetItemAsync(id) != null;

        private async Task<bool> AddItemAsync(Item item) => await _cartRepository.InsertItemAsync(item) != default;

        private async Task EnsureItemCreated(Item item)
        {
            if (!(await ItemExistsAsync(item.Id)))
            {
                if (!(await AddItemAsync(item)))
                {
                    throw new InvalidOperationException(DomainErrorMessagesProvider.ItemUnresolvedInsertOperationErrorMessage());
                }
            }
        }
    }
}
