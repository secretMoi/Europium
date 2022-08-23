using Europium.Models;
using Europium.Services.Apis.TheMovieDb.Models;
using Microsoft.Extensions.Options;

namespace Europium.Services.Apis.TheMovieDb;

public class SerieService : TheMovieDbService
{
	public SerieService(IOptions<AppConfig> options) : base(options)
	{
	}
	
	public async Task<Media?> GetSerieByNameAsync(string name)
	{
		using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 5));

		var parameters = GetUrlParameter();
		parameters.Add("query", name);
		parameters.Add("page", "1");
		
		var response = await _httpClient?.GetAsync(GetCompleteUri("3/search/tv", parameters), cts.Token)!;

		var serie = (await response.Content.ReadAsAsync<Medias>(cts.Token)).Results.FirstOrDefault();

		if (serie is null) return null;

		var serieById = await GetSerieByIdAsync(serie.Id);

		serie.Link = $"https://www.themoviedb.org/tv/{serie.Id}?language=fr";
		serie.Seasons = serieById.Seasons;
		serie.BackdropPath = _theMovieDb?.ImageBasePath + serie.BackdropPath;
		serie.PosterPath = _theMovieDb?.ImageBasePath + serie.PosterPath;
		serie.Title = serie.Name;

		return serie;
	}
	
	public async Task<SerieById> GetSerieByIdAsync(int serieId)
	{
		using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 5));

		var parameters = GetUrlParameter();
		
		var response = await _httpClient?.GetAsync(GetCompleteUri("3/tv/" + serieId, parameters), cts.Token)!;

		var serie = await response.Content.ReadAsAsync<SerieById>(cts.Token);

		foreach (var season in serie.Seasons)
		{
			if(season.PosterPath is not null)
				season.PosterPath = _theMovieDb?.ImageBasePath + season.PosterPath;
		}

		return serie;
	}
	
	public async Task<SerieIdLinkToOtherApi?> GetSerieIdLinkAsync(int serieId)
	{
		using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 5));

		var url = GetCompleteUri(
			$"3/tv/{serieId}/external_ids",
			GetUrlParameter()
			);
		
		var response = await _httpClient?.GetAsync(url, cts.Token)!;

		return await response.Content.ReadAsAsync<SerieIdLinkToOtherApi>(cts.Token);
	}
}