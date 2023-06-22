using System.Net;
using Europium.Models;
using Europium.Repositories.Models;
using Europium.Services.Apis.QBitTorrent;
using Europium.Services.Apis.YggTorrent;

namespace Europium.Repositories;

public class QBitTorrentRepository
{
	private static ApiToMonitor? _monitoredApi;
	private static HttpClient? _httpClient;
	private static CookieContainer? _cookies;

	public QBitTorrentRepository(ApisToMonitorRepository apisToMonitorRepository)
	{
		_monitoredApi ??= apisToMonitorRepository.GetApiByCode(ApiCode.QBITTORRENT);

		_cookies ??= new CookieContainer();

		if (_httpClient is null)
		{
			var handler = new HttpClientHandler();
			handler.CookieContainer = _cookies;
			_httpClient = new HttpClient(handler);
		}
	}
	
	public async Task<bool> IsUpAsync()
	{
		try
		{
			return await LoginAsync();
		}
		catch (Exception)
		{
			return false;
		}
	}
	
	public async Task<List<TorrentInfo>?> GetAllAsync()
	{
		await LoginAsync();
		var response = await _httpClient?.GetAsync(_monitoredApi?.Url + "/api/v2/torrents/info?filter=all", CancellationToken())!;

		if (response.StatusCode == HttpStatusCode.Forbidden)
			await LoginAsync();

		if (!response.IsSuccessStatusCode)
			return null;

		return await response.Content.ReadAsAsync<List<TorrentInfo>>(CancellationToken());
	}
	
	public async Task<bool> DeleteTorrentAsync(string torrentHash)
	{
		await LoginAsync();
		var formContent = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("hashes", torrentHash), 
			new KeyValuePair<string, string>("deleteFiles", "false") 
		});
		
		var httpResponseMessage = await _httpClient?.PostAsync(
			_monitoredApi?.Url + "/api/v2/torrents/delete",
			formContent,
			CancellationToken()
		)!;

		return httpResponseMessage.IsSuccessStatusCode;
	}
	
	public async Task<bool> AddTorrent(byte[] torrentFile, string torrentName, MediaType mediaType)
	{
		await LoginAsync();

		using var form = new MultipartFormDataContent();
		var fileContent = new ByteArrayContent(torrentFile);
		form.Add(fileContent, "torrents", torrentName);
		form.Add(new StringContent(GetCategory(mediaType)), "category");
		
		var response = await _httpClient?.PostAsync(_monitoredApi?.Url + "/api/v2/torrents/add", form)!;

		return response.IsSuccessStatusCode;
	}

	private async Task<bool> LoginAsync(bool skipLoginIfAlreadyLogged = false)
	{
		if (_cookies?.Count > 0 && skipLoginIfAlreadyLogged) return true;

		var logins = new List<KeyValuePair<string, string>>
		{
			new("username", _monitoredApi?.UserName ?? string.Empty),
			new("password", _monitoredApi?.Password ?? string.Empty)
		};

		var req = new HttpRequestMessage(HttpMethod.Post, _monitoredApi?.Url + "/api/v2/auth/login")
			{ Content = new FormUrlEncodedContent(logins) };
		var httpResponse = await _httpClient?.SendAsync(req, CancellationToken())!;
		var responseBody = await httpResponse.Content.ReadAsStringAsync(CancellationToken());
		return httpResponse.IsSuccessStatusCode && responseBody.Contains("Ok");
	}

	private CancellationToken CancellationToken()
	{
		return new CancellationTokenSource(new TimeSpan(0, 0, 5)).Token;
	}

	private string GetCategory(MediaType mediaType)
	{
		return mediaType switch
		{
			MediaType.Movie => "radarr",
			MediaType.Serie or MediaType.Anime => "tv-sonarr",
			_ => ""
		};
	}
}