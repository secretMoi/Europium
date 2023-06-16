using Europium.Repositories;
using Europium.Services.Apis.TheMovieDb.Models.Sonarr;

namespace Europium.Services.Apis;

public class SonarrService
{
	private readonly SonarrRepository _sonarrRepository;

	public SonarrService(SonarrRepository sonarrRepository)
	{
		_sonarrRepository = sonarrRepository;
	}
	
	public async Task<SonarrInformation?> GetSerieByTvdbIdAsync(int tvdbId)
	{
		return await _sonarrRepository.GetSerieByTvdbIdAsync(tvdbId);
	}

	public async Task<bool> IsUpAsync(string url)
	{
		return await _sonarrRepository.IsUpAsync(url);
	}
}