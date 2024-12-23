using Microsoft.AspNetCore.Mvc;
using MongoApp.Services;

namespace MongoApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MongoAppController : ControllerBase
{
    private IObjectService _objectService;

    public MongoAppController(IObjectService objectService)
    {
        _objectService = objectService;
    }

    [HttpGet("{key}")]
    public async Task<IActionResult> GetAsync([FromRoute] string key, CancellationToken cancellationToken)
        => Ok(await _objectService.GetByIdAsync(key, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> PostAsync(PostObjectRequest request, CancellationToken cancellationToken)
        => Ok(await _objectService.PostAsync(request, cancellationToken));

    [HttpDelete("{key}")]
    public async Task DeleteAsync([FromRoute] string key, CancellationToken cancellationToken)
        => await _objectService.DeleteAsync(key, cancellationToken);
}