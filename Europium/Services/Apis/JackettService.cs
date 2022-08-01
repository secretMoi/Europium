using Europium.Models;
using Europium.Repositories;

namespace Europium.Services.Apis;

public class JackettService : CommonApiService
{
	public JackettService(ApisToMonitorRepository apisToMonitorRepository) : base(apisToMonitorRepository)
	{
		var monitoredApi = _apisToMonitorRepository.GetApiByCode(ApiCode.JACKETT);
		// _httpClient.DefaultRequestHeaders.Add("X-Api-Key", monitoredApi?.ApiKey);
	}
	
	public virtual async Task<bool> IsUpAsync(string url)
	{
		try
		{
			using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 5));
			var response = await _httpClient.GetAsync(url + "/health", cts.Token);
		       
			return response.IsSuccessStatusCode;
		}
		catch (Exception)
		{
			return false;
		}
	}
}