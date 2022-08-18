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
		var movie = await _theMovieDbService.GetMovieByNameAsync(movieName);

		if (movie is null) return NotFound();
		
		return Ok(movie);
	}
	
	[HttpGet("serie/{serieName}")]
	public async Task<IActionResult> GetSerieByName(string serieName)
	{
		var serie = await _theMovieDbService.GetSerieByNameAsync(serieName);
		
		if (serie is null) return NotFound();
		
		return Ok(serie);
	}
}