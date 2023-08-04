using Europium.Models;
using Europium.Services.Apis;
using Europium.Services.Apis.TheMovieDb.Models.Radarr;

namespace Europium.Repositories;

public class RadarrRepository : CommonApiRepository
{
    public RadarrRepository(ApisToMonitorRepository apisToMonitorRepository) : base(apisToMonitorRepository)
    {
        _monitoredApi = _apisToMonitorRepository.GetApiByCode(ApiCode.RADARR);
        HttpClient.DefaultRequestHeaders.Add("X-Api-Key", _monitoredApi?.ApiKey);
    }
    
    public async Task<RadarrInformation?> GetMovieByTmdbIdAsync(int tmdbId)
    {
        var response = await HttpClient.GetAsync(_monitoredApi?.Url + "api/v3/movie?tmdbId=" + tmdbId, GetCancellationToken(5));
        var radarrInformation = (await response.Content.ReadAsAsync<RadarrInformation[]>(GetCancellationToken(5))).FirstOrDefault();

        if (radarrInformation is null) return null;
        radarrInformation.FileLink = $"{_monitoredApi?.Url}movie/{radarrInformation.TmdbId}";

        return radarrInformation;
    }
}