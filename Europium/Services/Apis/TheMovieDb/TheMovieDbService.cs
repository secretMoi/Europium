using Europium.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace Europium.Services.Apis.TheMovieDb;

public class TheMovieDbService
{
	protected static HttpClient? HttpClient;
	protected static TheMovieDbConfig? TheMovieDb;
	
	public TheMovieDbService(IOptions<AppConfig> options)
	{
		TheMovieDb = options.Value.TheMovieDb;
		HttpClient ??= new HttpClient(new HttpClientHandler());
	}

	private string GetUrl(string path)
	{
		return TheMovieDb?.ApiUrl + path;
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

	protected Uri GetCompleteUri(string url, IDictionary<string, string?> parameters)
	{
		return new Uri(QueryHelpers.AddQueryString(GetUrl(url), parameters));
	}
}