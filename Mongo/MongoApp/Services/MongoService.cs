using Microsoft.Extensions.Options;
using MongoApp.Options;
using MongoDB.Driver;

namespace MongoApp.Services;

public class MongoService
{
    private readonly IMongoDatabase _database;

    public MongoService(IOptions<MongoOptions> mongoOptions)
    {
        var mongoOptions1 = mongoOptions.Value;
        
        var url = MongoUrl.Create(mongoOptions1.ConnectionString);
        var client = new MongoClient(url);

        var databaseNames = client.ListDatabaseNames();
        _database = client.GetDatabase(mongoOptions1.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }

    public void CreateCollection(string collectionName)
    {
        _database.CreateCollection(collectionName);
    }
}