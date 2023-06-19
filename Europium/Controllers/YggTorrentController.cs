using Europium.Dtos;
using Europium.Services.Apis.YggTorrent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
    public async Task<IActionResult> GetRatio()
    {
        return Ok(await _yggTorrentService.GetRatio());
    }

    [HttpPost("search")]
    public async Task<IActionResult> SearchTorrentByName([FromBody] YggSearchParameterDto searchParameter)
    {
        return Ok(await _yggTorrentService.SearchByName(searchParameter.Search));
    }
}