using Europium.Models;
using Europium.Repositories;
using Europium.Repositories.Models;
using Microsoft.Extensions.Options;

namespace Europium.Services;

public class MonitorService
{
	private readonly RadarrService _radarrService;
	private readonly SonarrService _sonarrService;
	private readonly PlexService _plexService;
	private readonly ApisToMonitorRepository _monitorRepository;
	private readonly AppConfig AppConfig;
	
	public MonitorService(RadarrService radarrService, IOptions<AppConfig> optionsSnapshot, ApisToMonitorRepository monitorRepository, SonarrService sonarrService, PlexService plexService)
	{
		_radarrService = radarrService;
		_monitorRepository = monitorRepository;
		_sonarrService = sonarrService;
		_plexService = plexService;
		AppConfig = optionsSnapshot.Value;
	}

	public async Task<bool?> VerifySingleApiState(string code, string url)
	{
		if (ApiCode.RADARR.Equals(code))
		{
			return await _radarrService.IsUpAsync(url);
		}
		if (ApiCode.SONARR.Equals(code))
		{
			return await _sonarrService.IsUpAsync(url);
		}
		
		if (ApiCode.PLEX.Equals(code))
		{
			// var servers = await _plexService.PlexAccount.ServerSummaries();
			return await _plexService.IsUpAsync(url);
		}

		return null;
	}
	
	public async Task<byte[]> GetApiLogoAsync(string imageName)
	{
		return await File.ReadAllBytesAsync($"{AppConfig.ApiToMonitorImagePath}/{imageName}"); 
	}

	public async Task<ApiToMonitor?> GetApiByCodeAsync(string apiCode)
	{
		return await _monitorRepository.GetApiByCodeAsync(apiCode);
	}
}