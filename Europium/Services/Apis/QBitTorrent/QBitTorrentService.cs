using System.Net;
using Europium.Models;
using Europium.Repositories;
using Europium.Repositories.Models;

namespace Europium.Services.Apis.QBitTorrent;

public class QBitTorrentService
{
	private static ApiToMonitor? _monitoredApi;
	
	private static HttpClient? _httpClient;
	private static CookieContainer? _cookies;
	
	public QBitTorrentService(ApisToMonitorRepository apisToMonitorRepository)
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

	private async Task<bool> LoginAsync(bool skipLoginIfAlreadyLogged = false)
	{
		if (_cookies?.Count > 0 && skipLoginIfAlreadyLogged) return true;
		
		var logins = new List<KeyValuePair<string, string>>
		{
			new("username", _monitoredApi?.UserName ?? string.Empty),
			new("password", _monitoredApi?.Password ?? string.Empty)
		};

		var req = new HttpRequestMessage(HttpMethod.Post, _monitoredApi?.Url + "/api/v2/auth/login") { Content = new FormUrlEncodedContent(logins) };
		using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 5));
		var httpResponse = await _httpClient?.SendAsync(req, cts.Token)!;
		var responseBody = await httpResponse.Content.ReadAsStringAsync(cts.Token);
		return httpResponse.IsSuccessStatusCode && responseBody.Contains("Ok");
	}
	
	public async Task<List<TorrentInfo>> GetAllAsync()
	{
		await LoginAsync(true);
		using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 5));
		// _cookies.Add(new Uri(LoginUrl), _cookies.GetCookies(new Uri(LoginUrl)).First());
		var response = await _httpClient?.GetAsync(_monitoredApi?.Url + "/api/v2/torrents/info?filter=all", cts.Token)!;
	       
		return await response.Content.ReadAsAsync<List<TorrentInfo>>(cts.Token);
	}

	public async Task<bool> DeleteTorrentAsync(string torrentHash)
	{
		await LoginAsync(true);
		using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 5));
		await _httpClient?.GetAsync(_monitoredApi?.Url + $"/api/v2/torrents/delete?hashes={torrentHash}&deleteFiles=false", cts.Token)!;

		return true;
	}
}