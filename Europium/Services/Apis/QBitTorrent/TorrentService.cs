using Europium.Repositories;

namespace Europium.Services.Apis.QBitTorrent;

public class TorrentService : QBitTorrentService
{

	public TorrentService(ApisToMonitorRepository apisToMonitorRepository) : base(apisToMonitorRepository)
	{
	}

	public async Task<List<TorrentInfo>> GetAllAsync()
	{
		await LoginAsync();
		using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 5));
		// cookies.Add(new Uri(LoginUrl), cookies.GetCookies(new Uri(LoginUrl)).First());
		var response = await _httpClient.GetAsync(_monitoredApi.Url + "/api/v2/torrents/info?filter=all", cts.Token);
	       
		return await response.Content.ReadAsAsync<List<TorrentInfo>>(cts.Token);
	}

	public async Task<bool> DeleteTorrentAsync(string torrentHash)
	{
		await LoginAsync();
		using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 5));
		await _httpClient.GetAsync(_monitoredApi.Url + $"/api/v2/torrents/delete?hashes={torrentHash}&deleteFiles=false", cts.Token);

		return true;
	}
}