﻿using Europium.Services.Apis.TheMovieDb;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Europium.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class TheMovieDbController : ControllerBase
{
	private readonly MovieService _movieService;
	private readonly SerieService _serieService;

	public TheMovieDbController(MovieService movieService, SerieService serieService)
	{
		_movieService = movieService;
		_serieService = serieService;
	}
	
	[HttpGet("movie/{movieName}")]
	public async Task<IActionResult> GetMovieByName(string movieName)
	{
		var movie = await _movieService.GetMovieByNameAsync(movieName);

		if (movie is null) return NotFound();
		
		return Ok(movie);
	}
	
	[HttpGet("serie/{serieName}")]
	public async Task<IActionResult> GetSerieByName(string serieName)
	{
		var serie = await _serieService.GetSerieByNameAsync(serieName);
		
		if (serie is null) return NotFound();
		
		return Ok(serie);
	}
	
	[HttpGet("serie/{tmdbId}/links")]
	public async Task<IActionResult> GetSerieLinksBySerieId(int tmdbId)
	{
		var serie = await _serieService.GetSerieIdLinkAsync(tmdbId);
		
		if (serie is null) return NotFound();
		
		return Ok(serie);
	}
}