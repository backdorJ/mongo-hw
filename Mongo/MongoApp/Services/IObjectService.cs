using MongoApp.Models;

namespace MongoApp.Services;

public interface IObjectService
{
    public Task<string> GetByIdAsync(string key, CancellationToken cancellationToken);

    public Task<string> PostAsync(PostObjectRequest request, CancellationToken cancellationToken);

    public Task DeleteAsync(string key, CancellationToken cancellationToken);
}

public record PostObjectRequest(string String);