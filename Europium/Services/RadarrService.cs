using Europium.Models;
using Europium.Repositories;

namespace Europium.Services;

public class RadarrService
{
	private readonly ApisToMonitorRepository _apisToMonitorRepository;

	private readonly HttpClient _httpClient;

	public RadarrService(ApisToMonitorRepository apisToMonitorRepository)
	{
		_apisToMonitorRepository = apisToMonitorRepository;
		var monitoredApi = _apisToMonitorRepository.GetApiByCode(ApiCode.RADARR);
		
		_httpClient = new HttpClient(new HttpClientHandler());
		_httpClient.DefaultRequestHeaders.Add("X-Api-Key", monitoredApi?.ApiKey);
	}
	
	public async Task<bool> IsUpAsync(string url)
	{
		try
		{
			using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 5));
			var response = await _httpClient.GetAsync(url + "/api/v3/system/status", cts.Token);
		       
			return response.IsSuccessStatusCode;
		}
		catch (Exception)
		{
			return false;
		}
	}
}