using Europium.Models;
using Europium.Repositories;
using Europium.Repositories.Models;
using Europium.Services.Apis.FlareSolver;
using Europium.Services.Apis.QBitTorrent;
using Microsoft.Extensions.Options;

namespace Europium.Services.Apis;

public class MonitorService
{
	private readonly RadarrService _radarrService;
	private readonly SonarrService _sonarrService;
	private readonly JackettService _jackettService;
	private readonly QBitTorrentService _qBitTorrentService;
	private readonly TautulliService _tautulliService;
	private readonly FlareSolverService _flareSolverService;
	private readonly PlexService _plexService;

	private readonly ApisToMonitorRepository _monitorRepository;

	private readonly AppConfig _appConfig;

	public MonitorService(RadarrService radarrService, IOptions<AppConfig> optionsSnapshot,
		ApisToMonitorRepository monitorRepository, SonarrService sonarrService, PlexService plexService,
		JackettService jackettService, QBitTorrentService qBitTorrentService, TautulliService tautulliService,
		FlareSolverService flareSolverService)
	{
		_radarrService = radarrService;
		_monitorRepository = monitorRepository;
		_sonarrService = sonarrService;
		_plexService = plexService;
		_jackettService = jackettService;
		_qBitTorrentService = qBitTorrentService;
		_tautulliService = tautulliService;
		_flareSolverService = flareSolverService;
		_appConfig = optionsSnapshot.Value;
	}

	public async Task<bool?> VerifySingleApiState(string code, string url)
	{
		return code switch
		{
			ApiCode.RADARR => await _radarrService.IsUpAsync(url),
			ApiCode.SONARR => await _sonarrService.IsUpAsync(url),
			ApiCode.JACKETT => await _jackettService.IsUpAsync(url),
			ApiCode.PLEX => await _plexService.IsUpAsync(url),
			ApiCode.QBITTORRENT => await _qBitTorrentService.IsUpAsync(),
			ApiCode.TAUTULLI => await _tautulliService.IsUpAsync(url),
			ApiCode.FLARESOLVER => await _flareSolverService.IsUpAsync(url),
			_ => null
		};
	}

	public async Task<string?> GetApiLogoAsync(string imageName)
	{
		if (string.IsNullOrEmpty(imageName))
		{
			return null;
		}

		var byteImage = await File.ReadAllBytesAsync($"{_appConfig.ApiToMonitorImagePath}/{imageName}");
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

	public async Task<ApiToMonitor?> GetApiByCodeAsync(string apiCode)
	{
		return await _monitorRepository.GetApiByCodeAsync(apiCode);
	}

	public async Task<bool> SaveApiAsync(ApiToMonitor api)
	{
		return await _monitorRepository.SaveApiAsync(api);
	}
}