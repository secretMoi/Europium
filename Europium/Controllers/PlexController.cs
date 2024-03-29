﻿using Europium.Dtos.Plex;
using Europium.Services.Apis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Europium.Controllers;

[Authorize]
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
    
    [HttpDelete("delete/media/{mediaId}/file/{fileId}")]
    public async Task<IActionResult> DeleteMedia(int mediaId, int fileId)
    {
        return Ok(await _plexService.DeleteMedia(mediaId, fileId));
    }
    
    [HttpGet("thumbnail")]
    public async Task<IActionResult> GetThumbnail([FromQuery] PlexPictureParameters pictureParameters)
    {
        try
        {
            return Ok(await _plexService.GetThumbnail(pictureParameters));
        }
        catch (Exception)
        {
            return NotFound();
        }
    }
    
    [HttpGet("restart")]
    public IActionResult Restart()
    {
        return _plexService.Restart() ? NoContent() : StatusCode(500);
    }
    
    [HttpGet("medias/playing")]
    public async Task<IActionResult> GetPlayingMedias()
    {
        return Ok(await _plexService.GetPlayingMedias());
    }
    
    [HttpGet("medias/history")]
    public async Task<IActionResult> GetMediasHistory([FromQuery] PlexHistoryFilters plexHistoryFilters)
    {
        return Ok(await _plexService.GetMediasHistory(plexHistoryFilters));
    }
}