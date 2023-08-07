using Europium.Models;
using Europium.Repositories.FlareSolver.Models;

namespace Europium.Repositories.FlareSolver;

public class FlareSolverRepository : CommonApiRepository
{
    private readonly FlareSolverCommandFactory _flareSolverCommandFactory;
    private readonly List<FlareSolverCookie> _cookies = new ();

    public FlareSolverRepository(ApisToMonitorRepository apisToMonitorRepository, FlareSolverCommandFactory flareSolverCommandFactory) : base(apisToMonitorRepository)
    {
        _flareSolverCommandFactory = flareSolverCommandFactory;
        _monitoredApi = apisToMonitorRepository.GetApiByCode(ApiCode.FLARESOLVER);
    }
    
    public override async Task<bool> IsUpAsync(string url)
    {
        try
        {
            var response = await HttpClient.GetAsync(url, GetCancellationToken(5));
		       
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<(string userAgent, string cookie)> ConnectToSite(string url, string sessionName, string cookieName)
    {
        var cookie = _cookies.FirstOrDefault(x => x.Name == cookieName);
        if (cookie is not null)
        {
            if (cookie.Expiration < DateTime.Now)
                return (cookie.UserAgent, cookie.Value);
            
            _cookies.Remove(cookie);
        }

        await RemoveSessionIfExists(sessionName);
        var response = await HttpClient.PostAsJsonAsync(Url, _flareSolverCommandFactory.GetRequestCommand(url ,sessionName));
        var result = await response.Content.ReadAsAsync<FlareSolverResponse>();
        
        return ExtractCookiesInfo(result, cookieName);
    }
    
    private async Task<List<string>> ListSessions()
    {
        var response = await HttpClient.PostAsJsonAsync(Url, _flareSolverCommandFactory.GetSessionsListCommand());
        var result = await response.Content.ReadAsAsync<SessionsList>();
        return result.Sessions;
    }

    public async Task CreateSession(string sessionName)
    {
        await HttpClient.PostAsJsonAsync(Url, _flareSolverCommandFactory.GetCreateSessionCommand(sessionName));
    }

    private async Task RemoveSession(string sessionName)
    {
        await HttpClient.PostAsJsonAsync(_monitoredApi?.Url, _flareSolverCommandFactory.GetRemoveSessionCommand(sessionName));
    }

    private async Task RemoveSessionIfExists(string sessionName)
    {
        var sessions = await ListSessions();
        if (sessions.Any(x => x == sessionName))
            await RemoveSession(sessionName);
    }

    private (string userAgent, string cookie) ExtractCookiesInfo(FlareSolverResponse flareSolverResponse, string cookieName)
    {
        var cookie = flareSolverResponse.Solution.Cookies.First(x => x.Name == cookieName);
        
        AddCookieToCache(flareSolverResponse.Solution.UserAgent, cookie.Name, cookie.Value, DateTime.Now.AddMinutes(29));

        return (flareSolverResponse.Solution.UserAgent, $"{cookie.Name}={cookie.Value}");
    }

    private void AddCookieToCache(string userAgent, string name, string value, DateTime expiration)
    {
        _cookies.Add(new FlareSolverCookie(userAgent, name, value, expiration));
    }

    private string Url => _monitoredApi?.Url + "v1";
}

internal class FlareSolverCookie
{
    public string UserAgent { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public DateTime Expiration { get; set; }

    public FlareSolverCookie(string userAgent, string name, string value, DateTime expiration)
    {
        UserAgent = userAgent;
        Name = name;
        Value = value;
        Expiration = expiration;
    }
}