using Europium.Models;
using Europium.Services.Apis.TheMovieDb.Models.Tmdb;
using Microsoft.Extensions.Options;

namespace Europium.Repositories.TheMovieDb;

public class SerieRepository : TheMovieDbRepository
{
    public SerieRepository(IOptions<AppConfig> options) : base(options)
    {
    }
    
    public async Task<Media?> GetSerieByNameAsync(string name)
    {
        var parameters = GetUrlParameter();
        parameters.Add("query", name);
        parameters.Add("page", "1");

        var response = await HttpClient.GetAsync(GetCompleteUri("3/search/tv", parameters), GetCancellationToken(5));

        return (await response.Content.ReadAsAsync<Medias>(GetCancellationToken(5))).Results.FirstOrDefault();
    }

    public async Task<SerieById> GetSerieByIdAsync(int serieId)
    {
        var parameters = GetUrlParameter();

        var response = await HttpClient.GetAsync(GetCompleteUri("3/tv/" + serieId, parameters), GetCancellationToken(5));

        var serie = await response.Content.ReadAsAsync<SerieById>(GetCancellationToken(5));

        if (serie.Seasons is not null)
        {
            foreach (var season in serie.Seasons.Where(season => season.PosterPath is not null))
                season.PosterPath = TheMovieDb?.ImageBasePath + season.PosterPath;
        }

        return serie;
    }

    public async Task<SerieIdLinkToOtherApi?> GetSerieIdLinkAsync(int tmdbId)
    {
        var url = GetCompleteUri(
            $"3/tv/{tmdbId}/external_ids",
            GetUrlParameter()
        );

        var response = await HttpClient.GetAsync(url, GetCancellationToken(5));

        return await response.Content.ReadAsAsync<SerieIdLinkToOtherApi>(GetCancellationToken(5));
    }
}