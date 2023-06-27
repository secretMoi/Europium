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
	private static HttpClient? _httpClient;
	private static ApiToMonitor? _plexApi;
	
	public PlexRepository(ApisToMonitorRepository monitorRepository, EuropiumContext context, PlexMapper plexMapper)
	{
		_context = context;
		_plexMapper = plexMapper;
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
		var localUrl = _context.ApiUrls.First(x => x.ApiToMonitorId == _plexApi!.ApiToMonitorId && x.Url.Contains("aorus")).Url;
		var query = new Dictionary<string, string>
		{
			["duplicate"] = "1"
		};
		
		var response = await _httpClient?.GetStreamAsync(GetUri(localUrl + $"/library/sections/{sectionId}/all", query), GetCancellationToken())!;
		var xml = await XDocument.LoadAsync(response, LoadOptions.None, GetCancellationToken());

		return _plexMapper.MapDuplicates(xml);
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
}