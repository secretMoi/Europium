using Europium.Models;
using Europium.Repositories;
using Europium.Repositories.Models;
using Europium.Services.Apis.QBitTorrent;
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
	private readonly ApiUrlRepository _apiUrlRepository;
	
	private readonly AppConfig AppConfig;
	
	public MonitorService(RadarrService radarrService, IOptions<AppConfig> optionsSnapshot, ApisToMonitorRepository monitorRepository, SonarrService sonarrService, PlexService plexService, JackettService jackettService, QBitTorrentService qBitTorrentService, ApiUrlRepository apiUrlRepository)
	{
		_radarrService = radarrService;
		_monitorRepository = monitorRepository;
		_sonarrService = sonarrService;
		_plexService = plexService;
		_jackettService = jackettService;
		_qBitTorrentService = qBitTorrentService;
		_apiUrlRepository = apiUrlRepository;
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
	
	public async Task<string?> GetApiLogoAsync(string imageName)
	{
		if (string.IsNullOrEmpty(imageName))
		{
			return null;
		}

		var byteImage = await File.ReadAllBytesAsync($"{AppConfig.ApiToMonitorImagePath}/{imageName}");
		string imageFormat;

		if (imageName.EndsWith(".svg"))
		{
			imageFormat = "svg+xml";
		}
		else if (imageName.EndsWith(".png"))
		{
			imageFormat = "png";
		}
		else
		{
			throw new BadImageFormatException();
		}
		
		return $"data:image/{imageFormat};base64,{Convert.ToBase64String(byteImage)}";
	}

	public async Task<ApiToMonitor> GetApiByCodeAsync(string apiCode)
	{
		return await _monitorRepository.GetApiByCodeAsync(apiCode);
	}

	public async Task<bool> SaveApiAsync(ApiToMonitor api)
	{
		return await _monitorRepository.SaveApiAsync(api);
	}
}