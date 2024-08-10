namespace CartingService.BLL.Carting
{
    public interface ICartService
    {
        Task<(Cart Cart, List<Item> Items)> GetCartAsync(string oId);

        Task<bool> AddItemAsync(string oId, Item item);

        Task<bool> UpdateItemAsync(Item item);

        Task<int> RemoveItemAsync(string oId, int itemId);

        Task<Item> GetItemAsync(int id);
    }
}
