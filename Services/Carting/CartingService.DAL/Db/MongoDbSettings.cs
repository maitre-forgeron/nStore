namespace CartingService.DAL.Db
{
    public class MongoDbSettings
    {
        public const string MongoDbSettingsName = "MongoDbSettings";

        public required string CartingCollection { get; set; }

        public required string ItemsCollection { get; set; }

        public required string ConnectionString { get; set; }
    }
}
