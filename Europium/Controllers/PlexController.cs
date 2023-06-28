using Europium.Dtos.Plex;
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
    
    [HttpGet("duplicates/{libraryType}/{libraryId}")]
    public async Task<IActionResult> GetDuplicates(PlexLibraryType libraryType, int libraryId)
    {
        try
        {
            return Ok(await _plexService.GetDuplicates(libraryType, libraryId));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
    
    [HttpGet("libraries")]
    public async Task<IActionResult> GetLibraries()
    {
        return Ok(await _plexService.GetLibraries());
    }
}