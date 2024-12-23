using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using MongoApp.Models;
using MongoDB.Driver;

namespace MongoApp.Services;

public class ObjectService : IObjectService
{
    private readonly MongoService _mongoService;
    private readonly IMongoCollection<MyObject> _mongoCollection;
    private readonly IDistributedCache _distributedCache;

    public ObjectService(MongoService mongoService, IDistributedCache distributedCache)
    {
        _mongoService = mongoService;
        _distributedCache = distributedCache;
        
        _mongoService.CreateCollection("myObjects");
        _mongoCollection = _mongoService.GetCollection<MyObject>("myObjects");
    }

    public async Task<string> GetByIdAsync(string key, CancellationToken cancellationToken)
    {
        var objectArray = await _distributedCache.GetAsync(key, cancellationToken);

        if (objectArray == null)
            return string.Empty;

        var @object = Encoding.UTF8.GetString(objectArray);
        
        return @object;
    }

    public async Task<string> PostAsync(PostObjectRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.String))
            throw new ArgumentNullException(nameof(request.String));

        var foundInRedis = await _distributedCache.GetAsync(request.String, cancellationToken);

        if (foundInRedis != null)
            return Encoding.UTF8.GetString(foundInRedis);

        var id = Guid.NewGuid().ToString();
        
        await _mongoCollection.InsertOneAsync(new MyObject
        {
            String = request.String,
            Id = id
        }, cancellationToken: cancellationToken);

        await _distributedCache.SetStringAsync(request.String, id, cancellationToken);
        return id;
    }

    public async Task DeleteAsync(string key, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        var foundFromRedis = await _distributedCache.GetAsync(key, cancellationToken);

        if (foundFromRedis == null)
            return;

        await _distributedCache.RemoveAsync(key, cancellationToken);
        var filter = Builders<MyObject>.Filter.Eq(x => x.String, key);
        
        await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
    }
}