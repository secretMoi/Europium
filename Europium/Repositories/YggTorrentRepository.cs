using System.Net;
using Europium.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace Europium.Repositories;

public class YggTorrentRepository
{
    private static HttpClient? _httpClient;
    private static CookieContainer? _cookies;
    private readonly YggTorrentConfig _yggTorrent;
    private static DateTime? _loginExpiration;

    public YggTorrentRepository(IOptions<AppConfig> options)
    {
        _yggTorrent = options.Value.YggTorrent;

        _cookies ??= new CookieContainer();

        if (_httpClient is null)
        {
            var handler = new HttpClientHandler();
            handler.CookieContainer = _cookies;
            _httpClient = new HttpClient(handler);
        }
    }

    private async Task Login()
    {
        if (_loginExpiration.HasValue && DateTime.Now <= _loginExpiration.Value) return;

        var formContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("id", _yggTorrent.User),
            new KeyValuePair<string, string>("pass", _yggTorrent.Password)
        });

        var response = await _httpClient?.PostAsync(
            _yggTorrent.Url + "/user/login",
            formContent,
            GetCancellationToken()
        )!;

        if (response.IsSuccessStatusCode)
            _loginExpiration = DateTime.Now.AddHours(2);
    }

    public async Task<string> GetRatio()
    {
        await Login();

        return await _httpClient?.GetStringAsync(
            _yggTorrent.Url + "/user/ajax_usermenu",
            GetCancellationToken()
        )!;
    }

    public async Task<string> SearchTorrent(string torrentName)
    {
        await Login();

        var query = new Dictionary<string, string>
        {
            ["name"] = torrentName,
            ["do"] = "search",
            ["category"] = "2145",
        };

        var url = QueryHelpers.AddQueryString(_yggTorrent.Url + "/engine/search", query!);

        return await _httpClient?.GetStringAsync(
            url,
            GetCancellationToken()
        )!;
    }

    public string GetDownloadTorrentUrl(string torrentId)
    {
        return _yggTorrent.Url + "/engine/download_torrent?id=" + torrentId;
    }

    private CancellationToken GetCancellationToken()
    {
        return new CancellationTokenSource(new TimeSpan(0, 0, 10)).Token;
    }
}