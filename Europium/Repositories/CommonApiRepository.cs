using System.Net;
using Europium.Repositories.Models;

namespace Europium.Repositories;

public class CommonApiRepository : BaseApiRepository
{
	protected readonly ApisToMonitorRepository _apisToMonitorRepository;

	protected ApiToMonitor? _monitoredApi;

	public CommonApiRepository(ApisToMonitorRepository apisToMonitorRepository)
	{
		_apisToMonitorRepository = apisToMonitorRepository;
		
		var cookies = new CookieContainer();
		var handler = new HttpClientHandler();
		handler.CookieContainer = cookies;
		HttpClient = new HttpClient(handler);
	}
	
	public virtual async Task<bool> IsUpAsync(string url)
	{
		try
		{
			var response = await HttpClient.GetAsync(url + "/api/v3/system/status", GetCancellationToken(5));
		       
			return response.IsSuccessStatusCode;
		}
		catch (Exception)
		{
			return false;
		}
	}
}