﻿using Europium.Services.Apis.QBitTorrent;
using Microsoft.AspNetCore.Mvc;

namespace Europium.Controllers;

[ApiController]
[Route("[controller]")]
public class TorrentController : ControllerBase
{
	private readonly QBitTorrentService _torrentService;

	public TorrentController(QBitTorrentService torrentService)
	{
		_torrentService = torrentService;
	}
	
	[HttpGet("list")]
	public async Task<IActionResult> GetAllTorrents()
	{
		return Ok(await _torrentService.GetAllAsync());
	}

	[HttpPost("delete/{torrentHash}")]
	public async Task<IActionResult> GetAllTorrents(string torrentHash)
	{
		try
		{
			return Ok(await _torrentService.DeleteTorrentAsync(torrentHash));
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return BadRequest();
		}
	}
}