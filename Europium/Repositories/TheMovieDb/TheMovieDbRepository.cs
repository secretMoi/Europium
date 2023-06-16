using Europium.Models;
using Europium.Services.Apis;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace Europium.Repositories.TheMovieDb;

public class TheMovieDbRepository : BaseApiRepository
{
    protected static TheMovieDbConfig? TheMovieDb;
    
    public TheMovieDbRepository(IOptions<AppConfig> options)
    {
        TheMovieDb = options.Value.TheMovieDb;
    }

    private string GetUrl(string path)
    {
        return TheMovieDb?.ApiUrl + path;
    }

    protected Uri GetCompleteUri(string url, IDictionary<string, string?> parameters)
    {
        return new Uri(QueryHelpers.AddQueryString(GetUrl(url), parameters));
    }
    
    protected Dictionary<string, string?> GetUrlParameter()
    {
        return new Dictionary<string, string?>
        {
            { "api_key", TheMovieDb?.ApiKey },
            { "language", "fr-FR" },
            { "include_adult", "true" }
        };
    }
}