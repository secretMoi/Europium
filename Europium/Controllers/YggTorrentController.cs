using Europium.Services.Apis.YggTorrent;
using Microsoft.AspNetCore.Mvc;

namespace Europium.Controllers;

[ApiController]
[Route("[controller]")]
public class YggTorrentController : ControllerBase
{
    private readonly YggTorrentService _yggTorrentService;

    public YggTorrentController(YggTorrentService yggTorrentService)
    {
        _yggTorrentService = yggTorrentService;
    }

    [HttpGet("ratio")]
    public async Task<IActionResult> GetApisToMonitor()
    {
        return Ok(await _yggTorrentService.GetRatio());
    }
}