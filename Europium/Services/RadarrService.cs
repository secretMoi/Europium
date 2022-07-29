using Europium.Models;
using Europium.Repositories;

namespace Europium.Services;

public class RadarrService
{
	private readonly ApisToMonitorRepository _apisToMonitorRepository;
	public string BaseUrl { get; set; }

	private readonly HttpClient _httpClient;

	public RadarrService(ApisToMonitorRepository apisToMonitorRepository)
	{
		_apisToMonitorRepository = apisToMonitorRepository;
		var monitoredApi = _apisToMonitorRepository.GetApiByCode(ApiCode.RADARR);
		
		// BaseUrl = monitoredApi.
		
		_httpClient = new HttpClient(new HttpClientHandler());
		_httpClient.DefaultRequestHeaders.Add("X-Api-Key", monitoredApi?.ApiKey);
	}
	
	public bool IsUp()
	{
		var response = _httpClient.GetAsync(BaseUrl + "/api/v3/system/status").Result;

		return response.IsSuccessStatusCode;
	}
}