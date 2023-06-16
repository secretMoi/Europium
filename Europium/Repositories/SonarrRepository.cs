using Europium.Models;
using Europium.Services.Apis;
using Europium.Services.Apis.TheMovieDb.Models.Sonarr;

namespace Europium.Repositories;

public class SonarrRepository : CommonApiService
{
    public SonarrRepository(ApisToMonitorRepository apisToMonitorRepository) : base(apisToMonitorRepository)
    {
        _monitoredApi = _apisToMonitorRepository.GetApiByCode(ApiCode.SONARR);
        HttpClient.DefaultRequestHeaders.Add("X-Api-Key", _monitoredApi?.ApiKey);
    }
    
    public async Task<SonarrInformation?> GetSerieByTvdbIdAsync(int tvdbId)
    {
        var response = await HttpClient.GetAsync(_monitoredApi?.Url + "api/series/lookup?term=tvdb:" + tvdbId, GetCancellationToken(5));

        var sonarrInformation = (await response.Content.ReadAsAsync<SonarrInformation[]>(GetCancellationToken(5))).FirstOrDefault();

        if (sonarrInformation is null) return null;
        sonarrInformation.FileLink = $"{_monitoredApi?.Url}series/{sonarrInformation.TitleSlug}";

        return sonarrInformation;
    }
}