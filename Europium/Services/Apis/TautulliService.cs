using Europium.Models;
using Europium.Repositories;
using Europium.Repositories.Models;

namespace Europium.Services.Apis;

public class TautulliService
{
	protected readonly ApisToMonitorRepository _apisToMonitorRepository;

	protected readonly HttpClient _httpClient;

	protected ApiToMonitor? _monitoredApi;
	
	public TautulliService(ApisToMonitorRepository apisToMonitorRepository)
	{
		_apisToMonitorRepository = apisToMonitorRepository;
		_monitoredApi = _apisToMonitorRepository.GetApiByCode(ApiCode.TAUTULLI);
		
		var handler = new HttpClientHandler();
		_httpClient = new HttpClient(handler);
	}
	
	public async Task<bool> IsUpAsync(string url)
	{
		try
		{
			using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 5));
			var response = await _httpClient.GetAsync($"{url}/api/v2?apikey={_monitoredApi?.ApiKey}&cmd=status", cts.Token);
		       
			return response.IsSuccessStatusCode;
		}
		catch (Exception)
		{
			return false;
		}
	}
}