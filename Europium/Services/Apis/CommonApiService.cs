using System.Net;
using Europium.Repositories;
using Europium.Repositories.Models;

namespace Europium.Services.Apis;

public class CommonApiService
{
	protected readonly ApisToMonitorRepository _apisToMonitorRepository;

	protected readonly HttpClient _httpClient;

	protected ApiToMonitor? _monitoredApi;

	public CommonApiService(ApisToMonitorRepository apisToMonitorRepository)
	{
		_apisToMonitorRepository = apisToMonitorRepository;
		
		var cookies = new CookieContainer();
		var handler = new HttpClientHandler();
		handler.CookieContainer = cookies;
		_httpClient = new HttpClient(handler);
	}
	
	public virtual async Task<bool> IsUpAsync(string url)
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