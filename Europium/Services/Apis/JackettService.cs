using Europium.Models;
using Europium.Repositories;
using Europium.Repositories.Models;

namespace Europium.Services.Apis;

public class JackettService
{
	private static HttpClient? _httpClient;

	private static ApiToMonitor? _jackettApi;
	
	public JackettService(ApisToMonitorRepository monitorRepository)
	{
		_jackettApi ??= monitorRepository.GetApiByCode(ApiCode.JACKETT);
		
		if (_httpClient is null)
		{
			_httpClient = new HttpClient(new HttpClientHandler());
			_httpClient.DefaultRequestHeaders.Add("X-Api-Key", _jackettApi?.ApiKey);
		}
	}
	
	public async Task<bool> IsUpAsync(string url)
	{
		try
		{
			using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 5));
			var response = await _httpClient?.GetAsync(url + "/health", cts.Token)!;
		       
			return response.IsSuccessStatusCode;
		}
		catch (Exception)
		{
			return false;
		}
	}
}