using Europium.Models;
using Europium.Repositories.Models;

namespace Europium.Services;

public class MonitorService
{
	private readonly RadarrService _radarrService;
	
	public MonitorService(RadarrService radarrService)
	{
		_radarrService = radarrService;
	}
	
	public void VerifyAllApisState(List<ApiToMonitor> monitoredApis)
	{
		foreach(var monitoredApi in monitoredApis)
		{
			if (ApiCode.RADARR.Equals(monitoredApi.Code))
			{
				foreach (var apiUrl in monitoredApi.ApiUrls)
				{
					_radarrService.BaseUrl = apiUrl.Url;
					apiUrl.State = _radarrService.IsUp();
				}
			}
		}
	}
}