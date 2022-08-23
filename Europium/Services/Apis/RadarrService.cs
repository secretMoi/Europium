using Europium.Models;
using Europium.Repositories;
using Europium.Services.Apis.TheMovieDb.Models;

namespace Europium.Services.Apis;

public class RadarrService : CommonApiService
{
	public RadarrService(ApisToMonitorRepository apisToMonitorRepository) : base(apisToMonitorRepository)
	{
		_monitoredApi = _apisToMonitorRepository.GetApiByCode(ApiCode.RADARR);
		_httpClient.DefaultRequestHeaders.Add("X-Api-Key", _monitoredApi?.ApiKey);
	}

	public async Task<RadarrInformation?> GetMovieByTmdbIdAsync(int tmdbId)
	{
		using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 5));
		
		var response = await _httpClient.GetAsync(_monitoredApi?.Url + "api/v3/movie?tmdbId=" + tmdbId, cts.Token);

		var radarrInformation = (await response.Content.ReadAsAsync<RadarrInformation[]>(cts.Token)).FirstOrDefault();

		if (radarrInformation is null) return null;

		radarrInformation.FileLink = $"{_monitoredApi?.Url}movie/{radarrInformation.TmdbId}";

		return radarrInformation;
	}
}