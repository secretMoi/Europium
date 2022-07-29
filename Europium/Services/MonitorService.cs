using Europium.Dtos;
using Europium.Models;

namespace Europium.Services;

public class MonitorService
{
	private RadarrService _radarrService;
	
	public MonitorService(RadarrService radarrService)
	{
		_radarrService = radarrService;
	}
	
	public void VerifyAllApisState(List<MonitoredApiDto> monitoredApis)
	{
		foreach(var monitoredApi in monitoredApis)
		{
			if (ApiCode.RADARR.Equals(monitoredApi.Code))
			{
				foreach (var apiUrl in monitoredApi.ApiUrls)
				{
					_radarrService.BaseUrl = apiUrl.Url;
					monitoredApi.State = _radarrService.IsUp();
				}
			}
		}
	}
}