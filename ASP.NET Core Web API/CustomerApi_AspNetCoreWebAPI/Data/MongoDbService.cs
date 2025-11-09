using MongoDB.Driver;

namespace CustomerApi_AspNetCoreWebAPI.Data
{
    public class MongoDbService
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoDatabase? _database;

        public MongoDbService(IConfiguration configuration)
        {
            this._configuration = configuration;
            var connectionString = _configuration.GetConnectionString("MongoDbConnection");
            var mongoUrl = MongoUrl.Create(connectionString);
            var mongoClient = new MongoClient(mongoUrl);
            
            if (mongoClient != null)
            {
                _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
            }
        }

        // This is the shorthand form, it is equivalent to the above below
        public IMongoDatabase? Database => _database;
        //public IMongoDatabase? Database
        //{
        //    get { return _database; }
        //}
    }
}
 