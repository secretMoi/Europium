using System.Net;
using Europium.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace Europium.Repositories;

public class YggTorrentRepository
{
    private readonly FlareSolverRepository _flareSolverRepository;
    private static HttpClient? _httpClient;
    private static CookieContainer? _cookies;
    private static DateTime? _loginExpiration;
    private readonly YggTorrentConfig _yggTorrent;

    public YggTorrentRepository(IOptions<AppConfig> options, FlareSolverRepository flareSolverRepository)
    {
        _flareSolverRepository = flareSolverRepository;
        _yggTorrent = options.Value.YggTorrent;

        _cookies ??= new CookieContainer();

        if (_httpClient is null)
        {
            var handler = new HttpClientHandler();
            handler.CookieContainer = _cookies;
            _httpClient = new HttpClient(handler);
        }
    }

    public async Task<string> GetRatio()
    {
        await Login();

        return await _httpClient?.GetStringAsync(
            _yggTorrent.Url + "/user/ajax_usermenu",
            GetCancellationToken()
        )!;
    }

    public async Task<List<string>> SearchTorrents(string torrentName)
    {
        await Login();

        var query = new Dictionary<string, string>
        {
            ["name"] = torrentName,
            ["do"] = "search",
            ["category"] = "2145",
        };

        var counter = 0;
        var pages = new List<string>();
        bool hasResults;
        do
        {
            query["page"] = (50 * counter).ToString();

            var newPage = await SearchTorrent(query);
            hasResults = !newPage.Contains("Aucun résultat");
            
            if(hasResults) pages.Add(newPage);

            counter++;
        } while (hasResults);

        return pages;
    }

    public async Task<StreamContent> DownloadTorrentFile(int torrentId)
    {
        await Login();
        
        var responseFile = await _httpClient?.GetAsync(new Uri(_yggTorrent.Url + "/engine/download_torrent?id=" + torrentId))!;
        return new StreamContent(await responseFile.Content.ReadAsStreamAsync());
    }

    private async Task Login(bool hasAlreadyTried = false)
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

        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            var flareSolverResult = await _flareSolverRepository.ConnectToSite(_yggTorrent.Url, "ygg");
            _httpClient?.DefaultRequestHeaders.Remove("User-Agent");
            _httpClient?.DefaultRequestHeaders.Remove("Cookie");
            _httpClient?.DefaultRequestHeaders.UserAgent.ParseAdd(flareSolverResult.userAgent);
            _httpClient?.DefaultRequestHeaders.Add("Cookie", flareSolverResult.cookie);
            if(!hasAlreadyTried)
                await Login(true);
        }

        if (response.IsSuccessStatusCode)
            _loginExpiration = DateTime.Now.AddHours(2);
    }

    private async Task<string> SearchTorrent(IDictionary<string, string> query)
    {
        var url = QueryHelpers.AddQueryString(_yggTorrent.Url + "/engine/search", query!);
        return await _httpClient?.GetStringAsync(
            url,
            GetCancellationToken()
        )!;
    }

    private CancellationToken GetCancellationToken()
    {
        return new CancellationTokenSource(new TimeSpan(0, 0, 100)).Token;
    }
}