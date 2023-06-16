using Europium.Mappers;
using Europium.Models;
using Europium.Repositories;
using Europium.Repositories.TheMovieDb;
using Europium.Services.Apis.TheMovieDb.Models.Tmdb;
using Microsoft.Extensions.Options;

namespace Europium.Services.Apis.TheMovieDb;

public class SerieService : TheMovieDbService
{
	private readonly SerieRepository _serieRepository;
	private readonly SerieMapper _serieMapper;
	private readonly SonarrRepository _sonarrRepository;

	public SerieService(IOptions<AppConfig> options, SerieRepository serieRepository, SerieMapper serieMapper, SonarrRepository sonarrRepository) : base(options)
	{
		_serieRepository = serieRepository;
		_serieMapper = serieMapper;
		_sonarrRepository = sonarrRepository;
	}

	public async Task<Media?> GetSerieByNameAsync(string name)
	{
		var serie = await _serieRepository.GetSerieByNameAsync(name);
		if (serie is null) return null;

		var serieById = await _serieRepository.GetSerieByIdAsync(serie.Id);
		_serieMapper.MapSerie(serie, TheMovieDb?.ImageBasePath ?? "", serieById.Seasons);

		var serieIdLinkToOtherApi = await GetSerieIdLinkAsync(serie.Id);
		if (serieIdLinkToOtherApi != null)
			serie.SonarrInformation = await _sonarrRepository.GetSerieByTvdbIdAsync(serieIdLinkToOtherApi.TvdbId);

		return serie;
	}

	public async Task<SerieIdLinkToOtherApi?> GetSerieIdLinkAsync(int tmdbId)
	{
		return await _serieRepository.GetSerieIdLinkAsync(tmdbId);
	}
}