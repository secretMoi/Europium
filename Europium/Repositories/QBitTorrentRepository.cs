using System.Net;
using Europium.Models;
using Europium.Repositories.Models;
using Europium.Services.Apis.QBitTorrent;

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
		//await AddTorrent("");
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
	
	public async Task AddTorrent(string torrentFile)
	{
		await LoginAsync();
		MultipartFormDataContent formData = new MultipartFormDataContent("----------------------------6688794727912");
		
		formData.Add(new StringContent("https://www3.yggtorrent.do/engine/download_torrent?id=79791"), "urls");
		//formData.Add(new StringContent("https://torcache.net/torrent/3B1A1469C180F447B77021074DBBCCAEF62611E8.torrent"), "urls");
		
		//formData.Add(new StringContent("C:/Users/qBit/Downloads"), "savepath");
		// formData.Add(new StringContent("ui=" + _cookies?.GetAllCookies().First().Value), "cookie");
		// formData.Add(new StringContent("radarr"), "category");
		// formData.Add(new StringContent("true"), "skip_checking");
		// formData.Add(new StringContent("true"), "paused");
		//formData.Add(new StringContent("true"), "root_folder");
		
		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _monitoredApi?.Url + "/api/v2/torrents/add");
		request.Headers.Add("User-Agent", "Fiddler");
		request.Headers.Add("Cookie", "SID=" + _cookies?.GetAllCookies().First().Value);
		request.Content = formData;
		
		HttpResponseMessage response = await _httpClient?.SendAsync(request)!;
		string responseContent = await response.Content.ReadAsStringAsync();
		return;
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
}