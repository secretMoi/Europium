using Europium.Models;
using Europium.Repositories;
using Europium.Services.Apis.TheMovieDb.Models;
using Europium.Services.Apis.TheMovieDb.Models.Sonarr;

namespace Europium.Services.Apis;

public class SonarrService : CommonApiService
{
	public SonarrService(ApisToMonitorRepository apisToMonitorRepository) : base(apisToMonitorRepository)
	{
		_monitoredApi = _apisToMonitorRepository.GetApiByCode(ApiCode.SONARR);
		_httpClient.DefaultRequestHeaders.Add("X-Api-Key", _monitoredApi?.ApiKey);
	}
	
	public async Task<SonarrInformation?> GetSerieByTvdbIdAsync(int tvdbId)
	{
		using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 5));
		
		var response = await _httpClient.GetAsync(_monitoredApi?.Url + "api/series/lookup?term=tvdb:" + tvdbId, cts.Token);

		var sonarrInformation = (await response.Content.ReadAsAsync<SonarrInformation[]>(cts.Token)).FirstOrDefault();

		if (sonarrInformation is null) return null;

		sonarrInformation.FileLink = $"{_monitoredApi?.Url}series/{sonarrInformation.TitleSlug}";

		return sonarrInformation;
	}
}