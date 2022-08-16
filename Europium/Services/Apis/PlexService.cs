using Europium.Models;
using Europium.Repositories;
using Europium.Repositories.Models;
using Plex.Api.Factories;
using Plex.Library.ApiModels.Accounts;

namespace Europium.Services.Apis;

public class PlexService
{
	private static HttpClient? _httpClient;

	private static ApiToMonitor? _plexApi;

	private static PlexAccount? PlexAccount { get; set; }

	public PlexService(IPlexFactory plexFactory, ApisToMonitorRepository monitorRepository)
	{
		_plexApi ??= monitorRepository.GetApiByCode(ApiCode.PLEX);

		PlexAccount ??= plexFactory.GetPlexAccount(_plexApi?.ApiKey);

		if (_httpClient is null)
		{
			_httpClient = new HttpClient(new HttpClientHandler());
			_httpClient.DefaultRequestHeaders.Add("X-Api-Key", _plexApi?.ApiKey);
		}
	}

	public async Task<bool?> IsUpAsync(string url)
	{
		try
		{
			using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 5));
			var response = await _httpClient?.GetAsync(url + "?X-Plex-Token=" + _plexApi?.ApiKey, cts.Token)!;
		       
			return response.IsSuccessStatusCode;
		}
		catch (Exception)
		{
			return false;
		}
	}
}