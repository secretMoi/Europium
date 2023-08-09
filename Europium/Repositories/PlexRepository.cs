using System.Xml.Linq;
using Europium.Dtos.Plex;
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
	private readonly PlexHistoryMapper _plexHistoryMapper;
	private readonly PlexUserMapper _plexUserMapper;
	private readonly PlexDeviceMapper _plexDeviceMapper;
	private readonly IWebHostEnvironment _env;
	private static HttpClient? _httpClient;
	private static ApiToMonitor? _plexApi;
	private static string? _plexUrl;

	public PlexRepository(ApisToMonitorRepository monitorRepository, EuropiumContext context,
		PlexMapper plexMapper, PlexSessionMapper plexSessionMapper, PlexHistoryMapper plexHistoryMapper,
		PlexUserMapper plexUserMapper, PlexDeviceMapper plexDeviceMapper,
		IWebHostEnvironment env)
	{
		_context = context;
		_plexMapper = plexMapper;
		_plexSessionMapper = plexSessionMapper;
		_plexHistoryMapper = plexHistoryMapper;
		_plexUserMapper = plexUserMapper;
		_plexDeviceMapper = plexDeviceMapper;
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
	
	public async Task<Stream> GetMediaPicture(PlexPictureParameters pictureParameters)
	{
		var type = pictureParameters.IsArt ? "art" : "thumb";
		var size = (pictureParameters.Size ?? pictureParameters.Size ?? 150).ToString();
		var query = new Dictionary<string, string>
		{
			["width"] = size,
			["height"] = size,
			["url"] = $"/library/metadata/{pictureParameters.ParentId}/{type}/{pictureParameters.ThumbnailId}",
		};
		return await _httpClient?.GetStreamAsync(GetUri(_plexUrl + "/photo/:/transcode", query), GetCancellationToken())!;
	}

	public async Task<List<PlexPlayingMedia>> GetPlayingMedias()
	{
		var response = await _httpClient?.GetStreamAsync(GetUri(_plexUrl + "/status/sessions"), GetCancellationToken())!;
		var xml = await XDocument.LoadAsync(response, LoadOptions.None, GetCancellationToken());
		
		return _plexSessionMapper.MapPlayingMedias(xml);
	}

	public async Task<List<PlexMediaHistory>> GetHistory(PlexHistoryFilters filters)
	{
		var query = new Dictionary<string, string>
		{
			["sort"] = "viewedAt:desc"
		};
		if (filters.LibraryId.HasValue)
			query["librarySectionID"] = filters.LibraryId.Value.ToString();
		if (filters.UserId.HasValue)
			query["accountID"] = filters.UserId.Value.ToString();
		if (filters.Since.HasValue)
			query["viewedAt>"] = filters.Since.Value.ToString();

		var getHistory = _httpClient?.GetStreamAsync(GetUri(_plexUrl + "/status/sessions/history/all", query), GetCancellationToken());
		var getUsers = GetUsers();
		var getDevices = GetDevices();

		await Task.WhenAll(getHistory!, getUsers, getDevices);

		return await _plexHistoryMapper.MapMediasHistory(getHistory.Result, getUsers.Result, getDevices.Result);
	}

	private async Task<List<PlexUser>> GetUsers()
	{
		var response = await _httpClient?.GetStreamAsync(GetUri(_plexUrl + "/accounts"), GetCancellationToken())!;

		return await _plexUserMapper.MapUsers(response);
	}

	private async Task<List<PlexDevice>> GetDevices()
	{
		var response = await _httpClient?.GetStreamAsync(GetUri(_plexUrl + "/devices"), GetCancellationToken())!;

		return await _plexDeviceMapper.MapDevices(response);
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