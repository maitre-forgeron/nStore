using CartingService.BLL.Carting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CartingService.DAL.Db;

public class CartDbContext
{
    private readonly IMongoDatabase _database;
    private readonly MongoDbSettings _mongoDbSettings;

    public CartDbContext(IMongoClient mongoClient, IOptions<MongoDbSettings> mongoDbSettings)
    {
        _mongoDbSettings = mongoDbSettings.Value;
        _database = mongoClient.GetDatabase(_mongoDbSettings.ConnectionString);
    }

    public IMongoCollection<Cart> Carts => _database.GetCollection<Cart>(_mongoDbSettings.CartingCollection);

    public IMongoCollection<Item> Items => _database.GetCollection<Item>(_mongoDbSettings.ItemsCollection);
}
