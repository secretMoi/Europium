using Europium.Services.Apis.TheMovieDb;
using Microsoft.AspNetCore.Mvc;

namespace Europium.Controllers;

[ApiController]
[Route("[controller]")]
public class TheMovieDbController : ControllerBase
{
	private readonly TheMovieDbService _theMovieDbService;

	public TheMovieDbController(TheMovieDbService theMovieDbService)
	{
		_theMovieDbService = theMovieDbService;
	}
	
	[HttpGet("movie/{movieName}")]
	public async Task<IActionResult> GetMovieByName(string movieName)
	{
		return Ok(await _theMovieDbService.GetMovieByNameAsync(movieName));
	}
}