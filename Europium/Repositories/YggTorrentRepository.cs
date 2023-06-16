using System.Net;
using Europium.Models;
using Microsoft.Extensions.Options;

namespace Europium.Repositories;

public class YggTorrentRepository
{
    private static HttpClient? _httpClient;
    private static CookieContainer? _cookies;
    private readonly YggTorrentConfig _yggTorrent;

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
        var formContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("id", _yggTorrent.User), 
            new KeyValuePair<string, string>("pass", _yggTorrent.Password) 
        });
        
        await _httpClient?.PostAsync(
            _yggTorrent.Url + "/user/login",
            formContent,
            GetCancellationToken()
        )!;
    }

    public async Task<string> GetRatio()
    {
        await Login();
        
        return await _httpClient?.GetStringAsync(
            _yggTorrent.Url + "/user/ajax_usermenu",
            GetCancellationToken()
        )!;
    }

    private CancellationToken GetCancellationToken()
    {
        return new CancellationTokenSource(new TimeSpan(0, 0, 10)).Token;
    }
}