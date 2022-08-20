using Europium.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace Europium.Services.Apis.TheMovieDb;

public class TheMovieDbService
{
	protected static HttpClient? _httpClient;
	protected static TheMovieDbConfig? _theMovieDb;
	
	public TheMovieDbService(IOptions<AppConfig> options)
	{
		_theMovieDb = options.Value.TheMovieDb;
		_httpClient ??= new HttpClient(new HttpClientHandler());
	}

	private string GetUrl(string path)
	{
		return _theMovieDb?.ApiUrl + path;
	}

	protected Dictionary<string, string?> GetUrlParameter()
	{
		return new Dictionary<string, string?>
		{
			{ "api_key", _theMovieDb?.ApiKey },
			{ "language", "fr-FR" },
			{ "include_adult", "true" }
		};
	}

	protected Uri GetCompleteUri(string url, IDictionary<string, string?> parameters)
	{
		return new Uri(QueryHelpers.AddQueryString(GetUrl(url), parameters));
	}
}