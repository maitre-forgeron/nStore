using CartingService.BLL.Carting;
using CartingService.DAL.Db;
using MongoDB.Driver;

namespace CartingService.DAL.Carting
{
    public class CartRepository : ICartRepository
    {
        private readonly CartDbContext _context;

        public CartRepository(CartDbContext context) 
        {
            _context = context;
        }

        public async Task<Cart> GetCartAsync(string oId)
        {
            var filter = Builders<Cart>.Filter.Eq(c => c.CartId, oId);

            return await _context.Carts.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<Item> GetItemAsync(int id)
        {
            var filter = Builders<Item>.Filter.Eq(i => i.Id, id);

            return await _context.Items.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<List<Item>> GetItemsAsync(List<int> ids)
        {
            var filter = Builders<Item>.Filter.In(i => i.Id, ids);

            return await _context.Items.Find(filter).ToListAsync();
        }

        public async Task<string> InsertCartAsync(Cart cart)
        {
            await _context.Carts.InsertOneAsync(cart);

            return cart.CartId;
        }

        public async Task<int> InsertItemAsync(Item item)
        {
            await _context.Items.InsertOneAsync(item);

            return item.Id;
        }

        public async Task<bool> UpdateCartAsync(Cart cart)
        {
            var filter = Builders<Cart>.Filter.Eq(c => c.CartId, cart.CartId);

            var result = await _context.Carts.ReplaceOneAsync(filter, cart);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var filter = Builders<Item>.Filter.Eq(i => i.Id, item.Id);

            var result = await _context.Items.ReplaceOneAsync(filter, item);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}