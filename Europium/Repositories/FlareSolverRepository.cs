using Europium.Dtos.FlareSolver;
using Europium.Models;
using Europium.Repositories.Models;
using Europium.Services.Apis;

namespace Europium.Repositories;

public class FlareSolverRepository : BaseApiRepository
{
    private readonly ApiToMonitor? _monitoredApi;

    public FlareSolverRepository(ApisToMonitorRepository apisToMonitorRepository)
    {
        _monitoredApi = apisToMonitorRepository.GetApiByCode(ApiCode.FLARESOLVER);
    }

    public async Task<(string userAgent, string cookie)> ConnectToSite(string url, string sessionName)
    {
        var command = new FlareSolverCommand
        {
            Command = "request.get",
            Url = url,
            Session = sessionName,
            ReturnOnlyCookies = true,
            MaxTimeout = 60000
        };

        var response = await HttpClient.PostAsJsonAsync(_monitoredApi?.Url, command);
        var result = await response.Content.ReadAsAsync<FlareSolverResponse>();
        
        return ExtractCookiesInfo(result);
    }
    
    public async Task<List<string>> ListSessions()
    {
        var command = new FlareSolverCommand
        {
            Command = "sessions.list"
        };

        var response = await HttpClient.PostAsJsonAsync(_monitoredApi?.Url, command);
        var result = await response.Content.ReadAsAsync<SessionsList>();
        return result.Sessions;
    }

    public async Task CreateSession(string sessionName)
    {
        var command = new FlareSolverCommand
        {
            Command = "sessions.create",
            Session = sessionName
        };

        await HttpClient.PostAsJsonAsync(_monitoredApi?.Url, command);
    }

    public async Task RemoveSession(string sessionName)
    {
        var command = new FlareSolverCommand
        {
            Command = "sessions.destroy",
            Session = sessionName
        };

        await HttpClient.PostAsJsonAsync(_monitoredApi?.Url, command);
    }

    private (string userAgent, string cookie) ExtractCookiesInfo(FlareSolverResponse flareSolverResponse)
    {
        var cookie = flareSolverResponse.Solution.Cookies.FirstOrDefault(x => x.Name == "cf_clearance");

        return (flareSolverResponse.Solution.UserAgent, $"{cookie?.Name}={cookie?.Value}");
    }
}