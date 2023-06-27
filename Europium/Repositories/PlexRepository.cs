using System.Xml.Linq;
using Europium.Dtos.Plex;
using Europium.Mappers;
using Europium.Models;
using Europium.Repositories.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace Europium.Repositories;

public class PlexRepository
{
	private readonly EuropiumContext _context;
	private readonly PlexMapper _plexMapper;
	private readonly IWebHostEnvironment _env;
	private static HttpClient? _httpClient;
	private static ApiToMonitor? _plexApi;
	
	public PlexRepository(ApisToMonitorRepository monitorRepository, EuropiumContext context, PlexMapper plexMapper, IWebHostEnvironment env)
	{
		_context = context;
		_plexMapper = plexMapper;
		_env = env;
		_plexApi ??= monitorRepository.GetApiByCode(ApiCode.PLEX);

		if (_httpClient is null)
		{
			_httpClient = new HttpClient(new HttpClientHandler());
			_httpClient.DefaultRequestHeaders.Add("X-Api-Key", _plexApi?.ApiKey);
		}
	}
	
	public async Task<bool?> IsUpAsync(string url)
	{
		try
		{
			var response = await _httpClient?.GetAsync(GetUri(url), GetCancellationToken())!;
			
			return response.IsSuccessStatusCode;
		}
		catch (Exception)
		{
			return false;
		}
	}
	
	public async Task<List<PlexDuplicateDto>> GetDuplicates(int sectionId)
	{
		var query = new Dictionary<string, string>
		{
			["duplicate"] = "1"
		};
		
		var response = await _httpClient?.GetStreamAsync(GetUri(GetPlexUrl() + $"/library/sections/{sectionId}/all", query), GetCancellationToken())!;
		var xml = await XDocument.LoadAsync(response, LoadOptions.None, GetCancellationToken());

		return _plexMapper.MapDuplicates(xml);
	}

	public async Task<List<PlexLibraryDto>> GetLibraries()
	{
		var response = await _httpClient?.GetStreamAsync(GetUri(GetPlexUrl() + "/library/sections"), GetCancellationToken())!;
		var xml = await XDocument.LoadAsync(response, LoadOptions.None, GetCancellationToken());

		return _plexMapper.MapLibraries(xml);
	}
	
	private void AddToken(IDictionary<string, string> parameters)
	{
		parameters.Add("X-Plex-Token", _plexApi?.ApiKey!);
	}

	private CancellationToken GetCancellationToken()
	{
		return new CancellationTokenSource(new TimeSpan(0, 0, 5)).Token;
	}

	private string GetUri(string url, IDictionary<string, string>? query = null)
	{
		query ??= new Dictionary<string, string>();
		
		AddToken(query);
		return QueryHelpers.AddQueryString(url, query!);
	}

	private string GetPlexUrl()
	{
		var urlToUse = _env.IsDevelopment() ? "ovh" : "localhost";
		return _context.ApiUrls.First(x => x.ApiToMonitorId == _plexApi!.ApiToMonitorId && x.Url.Contains(urlToUse)).Url;
	}
}