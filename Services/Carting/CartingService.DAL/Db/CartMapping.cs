using CartingService.BLL.Carting;
using MongoDB.Bson.Serialization;

namespace CartingService.DAL.Db;

public class CartMapping
{
    public static void Map()
    {
        BsonClassMap.RegisterClassMap<Cart>(cm =>
        {
            cm.AutoMap();
            cm.MapIdMember(c => c.CartId);
        });
    }
}
