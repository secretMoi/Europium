using Europium.Models;
using Europium.Repositories;
using Europium.Repositories.Models;
using Plex.Api.Factories;
using Plex.Library.ApiModels.Accounts;

namespace Europium.Services;

public class PlexService
{
	private readonly ApisToMonitorRepository _monitorRepository;
	private readonly HttpClient _httpClient;

	private readonly ApiToMonitor _plexApi;
	
	public PlexAccount PlexAccount { get; }

	public PlexService(IPlexFactory plexFactory, ApisToMonitorRepository monitorRepository)
	{
		_monitorRepository = monitorRepository;

		_plexApi = _monitorRepository.GetApiByCode(ApiCode.PLEX);

		PlexAccount = plexFactory.GetPlexAccount(_plexApi.ApiKey);
		
		var monitoredApi = _monitorRepository.GetApiByCode(ApiCode.SONARR);
		
		_httpClient = new HttpClient(new HttpClientHandler());
		_httpClient.DefaultRequestHeaders.Add("X-Api-Key", monitoredApi?.ApiKey);
	}

	public async Task<bool?> IsUpAsync(string url)
	{
		try
		{
			using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 5));
			var response = await _httpClient.GetAsync(url + "?X-Plex-Token=" + _plexApi.ApiKey, cts.Token);
		       
			return response.IsSuccessStatusCode;
		}
		catch (Exception)
		{
			return false;
		}
	}
}