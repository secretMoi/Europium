using Europium.Models;
using Europium.Repositories;

namespace Europium.Services.Apis.QBitTorrent;

public class QBitTorrentService : CommonApiService
{
	public QBitTorrentService(ApisToMonitorRepository apisToMonitorRepository) : base(apisToMonitorRepository)
	{
		_monitoredApi = _apisToMonitorRepository.GetApiByCode(ApiCode.QBITTORRENT);
	}
	
	public override async Task<bool> IsUpAsync(string url)
	{
		try
		{
			var res = await LoginAsync();
			
			var responseBody = await res.Content.ReadAsStringAsync();
			return res.IsSuccessStatusCode && responseBody.Contains("Ok");
		}
		catch (Exception)
		{
			return false;
		}
	}

	protected async Task<HttpResponseMessage> LoginAsync()
	{
		var logins = new List<KeyValuePair<string, string>>
		{
			new("username", _monitoredApi.UserName),
			new("password", _monitoredApi.Password)
		};

		var req = new HttpRequestMessage(HttpMethod.Post, _monitoredApi.Url + "/api/v2/auth/login") { Content = new FormUrlEncodedContent(logins) };
		using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 5));
		return await _httpClient.SendAsync(req, cts.Token);
	}
}