using Europium.Services.Apis;
using Microsoft.AspNetCore.Mvc;

namespace Europium.Controllers;

[ApiController]
[Route("[controller]")]
public class PlexController : ControllerBase
{
    private readonly PlexService _plexService;

    public PlexController(PlexService plexService)
    {
        _plexService = plexService;
    }
    
    [HttpGet("duplicates/{libraryId}")]
    public async Task<IActionResult> GetDuplicates(int libraryId)
    {
        try
        {
            return Ok(await _plexService.GetDuplicates(libraryId));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}