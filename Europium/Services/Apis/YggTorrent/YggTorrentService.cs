using System.Net;

namespace Europium.Services.Apis.YggTorrent;

public class YggTorrentService
{
    private static HttpClient? _httpClient;
    private static CookieContainer? _cookies;

    public YggTorrentService()
    {
        _cookies ??= new CookieContainer();
        
        if (_httpClient is null)
        {
            var handler = new HttpClientHandler();
            handler.CookieContainer = _cookies;
            _httpClient = new HttpClient(handler);
        }
    }
    
    public async Task<bool> GetRatio()
    {
        await Login();
        
        using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 20));
        var t = await _httpClient?.GetStringAsync(
            "https://www3.yggtorrent.do/user/ajax_usermenu",
            cts.Token
        )!;

        t = t.Substring(t.IndexOf("ico_upload") + "ico_upload".Length);
        t = t.Substring(0, t.IndexOf("Compte"));

        t = CleanResponse(t);

        return true;
    }

    private async Task Login()
    {
        using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 20));
        var formContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("id", "ygguser"), 
            new KeyValuePair<string, string>("pass", "yggpass") 
        });
        
        await _httpClient?.PostAsync(
            "https://www3.yggtorrent.do/user/login",
            formContent,
            cts.Token
        )!;
    }

    private string CleanResponse(string response)
    {
        while (response.Contains('<'))
        {
            int start = response.LastIndexOf('<');
            int end = response.IndexOf('>', start);
            response = response.Remove(start, end - start + 1);
        }

        return response;
    }
}