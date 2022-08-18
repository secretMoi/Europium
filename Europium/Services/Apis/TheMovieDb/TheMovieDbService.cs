using Europium.Dtos;
using Europium.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace Europium.Services.Apis.TheMovieDb;

public class TheMovieDbService
{
	private static HttpClient? _httpClient;
	private static Models.TheMovieDb? _theMovieDb;
	
	public TheMovieDbService(IOptions<AppConfig> options)
	{
		_theMovieDb = options.Value.TheMovieDb;
		_httpClient ??= new HttpClient(new HttpClientHandler());
	}

	public async Task<Movie?> GetMovieByNameAsync(string name)
	{
		using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 5));

		var parameters = GetUrlParameter();
		parameters.Add("query", name);
		
		var response = await _httpClient?.GetAsync(GetCompleteUri("search/movie", parameters), cts.Token)!;

		var movie = (await response.Content.ReadAsAsync<Movies>(cts.Token)).Results.FirstOrDefault();

		if (movie is null) return null;

		movie.BackdropPath = _theMovieDb?.ImageBasePath + movie.BackdropPath;
		movie.PosterPath = _theMovieDb?.ImageBasePath + movie.PosterPath;

		return movie;
	}

	public async Task<Movie?> GetSerieByNameAsync(string name)
	{
		using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 5));

		var parameters = GetUrlParameter();
		parameters.Add("query", name);
		
		var response = await _httpClient?.GetAsync(GetCompleteUri("search/tv", parameters), cts.Token)!;

		var movie = (await response.Content.ReadAsAsync<Movies>(cts.Token)).Results.FirstOrDefault();

		if (movie is null) return null;

		movie.BackdropPath = _theMovieDb?.ImageBasePath + movie.BackdropPath;
		movie.PosterPath = _theMovieDb?.ImageBasePath + movie.PosterPath;
		movie.Title = movie.Name;

		return movie;
	}

	private string GetUrl(string path)
	{
		return _theMovieDb?.ApiUrl + path;
	}

	private Dictionary<string, string?> GetUrlParameter()
	{
		return new Dictionary<string, string?>
		{
			{ "api_key", _theMovieDb?.ApiKey }
		};
	}

	private Uri GetCompleteUri(string url, IDictionary<string, string?> parameters)
	{
		return new Uri(QueryHelpers.AddQueryString(GetUrl(url), parameters));
	}
}