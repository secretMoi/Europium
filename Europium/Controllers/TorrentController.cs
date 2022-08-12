using Europium.Services.Apis.QBitTorrent;
using Microsoft.AspNetCore.Mvc;

namespace Europium.Controllers;

[ApiController]
[Route("[controller]")]
public class TorrentController : ControllerBase
{
	private readonly TorrentService _torrentService;

	public TorrentController(TorrentService torrentService)
	{
		_torrentService = torrentService;
	}
	
	[HttpGet("list")]
	public async Task<IActionResult> GetAllTorrents()
	{
		return Ok(await _torrentService.GetAllAsync());
	}
}