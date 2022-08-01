using Europium.Repositories;

namespace Europium.Services.Apis;

public class JackettService : CommonApiService
{
	public JackettService(ApisToMonitorRepository apisToMonitorRepository) : base(apisToMonitorRepository)
	{
	}
	
	public override async Task<bool> IsUpAsync(string url)
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