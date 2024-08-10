namespace CartingService.BLL.Carting
{
    public interface ICartRepository
    {
        Task<Cart> GetCartAsync(string oId);

        Task<Item> GetItemAsync(int id);

        Task<bool> UpdateItemAsync(Item item);

        Task<List<Item>> GetItemsAsync(List<int> ids);

        Task<string> InsertCartAsync(Cart cart);

        Task<int> InsertItemAsync(Item item);

        Task<bool> UpdateCartAsync(Cart cart);
    }
}
