using Europium.Repositories;
using Europium.Services.Apis.TheMovieDb.Models.Radarr;

namespace Europium.Services.Apis;

public class RadarrService
{
	private readonly RadarrRepository _radarrRepository;

	public RadarrService(RadarrRepository radarrRepository)
	{
		_radarrRepository = radarrRepository;
	}

	public async Task<RadarrInformation?> GetMovieByTmdbIdAsync(int tmdbId)
	{
		return await _radarrRepository.GetMovieByTmdbIdAsync(tmdbId);
	}

	public async Task<bool> IsUpAsync(string url)
	{
		return await _radarrRepository.IsUpAsync(url);
	}
}