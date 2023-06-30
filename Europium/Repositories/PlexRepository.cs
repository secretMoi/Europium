using System.Xml.Linq;
using Europium.Dtos.Plex;
using Europium.Mappers;
using Europium.Mappers.Plex;
using Europium.Models;
using Europium.Repositories.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace Europium.Repositories;

public class PlexRepository
{
	private readonly EuropiumContext _context;
	private readonly PlexMapper _plexMapper;
	private readonly PlexSessionMapper _plexSessionMapper;
	private readonly IWebHostEnvironment _env;
	private static HttpClient? _httpClient;
	private static ApiToMonitor? _plexApi;
	private static string? _plexUrl;

	public PlexRepository(ApisToMonitorRepository monitorRepository, EuropiumContext context, PlexMapper plexMapper, PlexSessionMapper plexSessionMapper, IWebHostEnvironment env)
	{
		_context = context;
		_plexMapper = plexMapper;
		_plexSessionMapper = plexSessionMapper;
		_env = env;
		
		_plexApi ??= monitorRepository.GetApiByCode(ApiCode.PLEX);
		_plexUrl ??= GetPlexUrl();

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
	
	public async Task<List<PlexDuplicate>> GetDuplicates(PlexLibraryType libraryType, int sectionId)
	{
		var query = new Dictionary<string, string> { ["duplicate"] = "1" };
		if (libraryType == PlexLibraryType.Serie)
			query.Add("type", "4");

		var response = await _httpClient?.GetStreamAsync(GetUri(_plexUrl + $"/library/sections/{sectionId}/all", query), GetCancellationToken())!;
		var xml = await XDocument.LoadAsync(response, LoadOptions.None, GetCancellationToken());

		return _plexMapper.MapDuplicates(xml, libraryType);
	}

	public async Task<List<PlexLibrary>> GetLibraries()
	{
		var response = await _httpClient?.GetStreamAsync(GetUri(_plexUrl + "/library/sections"), GetCancellationToken())!;
		var xml = await XDocument.LoadAsync(response, LoadOptions.None, GetCancellationToken());

		return _plexMapper.MapLibraries(xml);
	}

	public async Task<bool> DeleteMedia(int mediaId, int fileId)
	{
		var response = await _httpClient?.DeleteAsync(GetUri(_plexUrl + $"/library/metadata/{mediaId}/media/{fileId}"), GetCancellationToken())!;
		return response.IsSuccessStatusCode;
	}
	
	public async Task<Stream> GetThumbnail(int parentId, int thumbnailId)
	{
		var query = new Dictionary<string, string>
		{
			["width"] = "180",
			["height"] = "145",
			["url"] = $"/library/metadata/{parentId}/art/{thumbnailId}",
		};
		return await _httpClient?.GetStreamAsync(GetUri(_plexUrl + "/photo/:/transcode", query))!;
	}

	public async Task<List<PlexPlayingMedia>> GetPlayingMedias()
	{
		var response = await _httpClient?.GetStreamAsync(GetUri(_plexUrl + "/status/sessions"), GetCancellationToken())!;
		var xml = await XDocument.LoadAsync(response, LoadOptions.None, GetCancellationToken());
		
		return _plexSessionMapper.MapPlayingMedias(xml);
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