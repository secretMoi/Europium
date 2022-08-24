using Europium.Models;
using Europium.Services.Apis.TheMovieDb.Models.Tmdb;
using Microsoft.Extensions.Options;

namespace Europium.Services.Apis.TheMovieDb;

public class MovieService : TheMovieDbService
{
	private readonly RadarrService _radarrService;

	public MovieService(IOptions<AppConfig> options, RadarrService radarrService) : base(options)
	{
		_radarrService = radarrService;
	}
	
	public async Task<Media?> GetMovieByNameAsync(string name)
	{
		using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 5));

		var parameters = GetUrlParameter();
		parameters.Add("query", name);
		parameters.Add("page", "1");
		
		var response = await _httpClient?.GetAsync(GetCompleteUri("3/search/movie", parameters), cts.Token)!;

		var movie = (await response.Content.ReadAsAsync<Medias>(cts.Token)).Results.FirstOrDefault();

		if (movie is null) return null;

		movie.Link = $"https://www.themoviedb.org/movie/{movie.Id}?language=fr";
		movie.BackdropPath = _theMovieDb?.ImageBasePath + movie.BackdropPath;
		movie.PosterPath = _theMovieDb?.ImageBasePath + movie.PosterPath;
		movie.RadarrInformation = await _radarrService.GetMovieByTmdbIdAsync(movie.Id);

		return movie;
	}
}