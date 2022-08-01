using Europium.Models;
using Europium.Repositories;
using Europium.Repositories.Models;
using Microsoft.Extensions.Options;

namespace Europium.Services.Apis;

public class MonitorService
{
	private readonly RadarrService _radarrService;
	private readonly SonarrService _sonarrService;
	private readonly JackettService _jackettService;
	private readonly QBitTorrentService _qBitTorrentService;
	private readonly PlexService _plexService;
	private readonly ApisToMonitorRepository _monitorRepository;
	private readonly AppConfig AppConfig;
	
	public MonitorService(RadarrService radarrService, IOptions<AppConfig> optionsSnapshot, ApisToMonitorRepository monitorRepository, SonarrService sonarrService, PlexService plexService, JackettService jackettService, QBitTorrentService qBitTorrentService)
	{
		_radarrService = radarrService;
		_monitorRepository = monitorRepository;
		_sonarrService = sonarrService;
		_plexService = plexService;
		_jackettService = jackettService;
		_qBitTorrentService = qBitTorrentService;
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
		if (ApiCode.JACKETT.Equals(code))
		{
			return await _jackettService.IsUpAsync(url);
		}
		if (ApiCode.PLEX.Equals(code))
		{
			// var servers = await _plexService.PlexAccount.ServerSummaries();
			return await _plexService.IsUpAsync(url);
		}
		if (ApiCode.QBITTORRENT.Equals(code))
		{
			return await _qBitTorrentService.IsUpAsync(url);
		}

		return null;
	}
	
	public async Task<byte[]> GetApiLogoAsync(string imageName)
	{
		if (String.IsNullOrEmpty(imageName))
		{
			return new byte[] { };
		}
		
		return await File.ReadAllBytesAsync($"{AppConfig.ApiToMonitorImagePath}/{imageName}"); 
	}

	public async Task<ApiToMonitor?> GetApiByCodeAsync(string apiCode)
	{
		return await _monitorRepository.GetApiByCodeAsync(apiCode);
	}
}