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
		try
		{
			using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 1));
			var response = _httpClient.GetAsync(BaseUrl + "/api/v3/system/status", cts.Token).Result;
		       
			return response.IsSuccessStatusCode;
		}
		catch (Exception)
		{
			return false;
		}
	}
}