using Europium.Models;
using Europium.Repositories;
using Europium.Repositories.Models;
using Microsoft.Extensions.Options;

namespace Europium.Services;

public class MonitorService
{
	private readonly RadarrService _radarrService;
	private readonly SonarrService _sonarrService;
	private readonly ApisToMonitorRepository _monitorRepository;
	private readonly AppConfig AppConfig;
	
	public MonitorService(RadarrService radarrService, IOptions<AppConfig> optionsSnapshot, ApisToMonitorRepository monitorRepository, SonarrService sonarrService)
	{
		_radarrService = radarrService;
		_monitorRepository = monitorRepository;
		_sonarrService = sonarrService;
		AppConfig = optionsSnapshot.Value;
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
			if (ApiCode.SONARR.Equals(monitoredApi.Code))
			{
				foreach (var apiUrl in monitoredApi.ApiUrls)
				{
					_sonarrService.BaseUrl = apiUrl.Url;
					apiUrl.State = _sonarrService.IsUp();
				}
			}
		}
	}

	public async Task<byte[]> GetApiLogoAsync(string imageName)
	{
		return await System.IO.File.ReadAllBytesAsync($"{AppConfig.ApiToMonitorImagePath}/{imageName}"); 
	}

	public async Task<ApiToMonitor?> GetApiByCodeAsync(string apiCode)
	{
		return await _monitorRepository.GetApiByCodeAsync(apiCode);
	}
}