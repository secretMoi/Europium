using Europium.Models;
using Europium.Repositories;

namespace Europium.Services.Apis;

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
			var logins = new List<KeyValuePair<string, string>>
			{
				new("username", _monitoredApi.UserName),
				new("password", _monitoredApi.Password)
			};
			
			var req = new HttpRequestMessage(HttpMethod.Post, url + "/api/v2/auth/login") { Content = new FormUrlEncodedContent(logins) };
			using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 5));
			var res = await _httpClient.SendAsync(req, cts.Token);
			
			var responseBody = await res.Content.ReadAsStringAsync(cts.Token);
			return res.IsSuccessStatusCode && responseBody.Contains("Ok");
		}
		catch (Exception)
		{
			return false;
		}
	}
}