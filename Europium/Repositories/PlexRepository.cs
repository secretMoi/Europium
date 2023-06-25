using Europium.Models;
using Europium.Repositories.Models;
using Plex.Api.Factories;
using Plex.Library.ApiModels.Accounts;
using Plex.Library.ApiModels.Libraries;
using Plex.Library.ApiModels.Servers;

namespace Europium.Repositories;

public class PlexRepository
{
	
	private static HttpClient? _httpClient;
	private static ApiToMonitor? _plexApi;
	//private static PlexAccount? PlexAccount { get; set; }
	
	public PlexRepository(/*IPlexFactory plexFactory, */ApisToMonitorRepository monitorRepository)
	{
		_plexApi ??= monitorRepository.GetApiByCode(ApiCode.PLEX);

		//PlexAccount ??= plexFactory.GetPlexAccount(_plexApi?.ApiKey);

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
			using var cts = new CancellationTokenSource(new TimeSpan(0, 0, 5));
			var response = await _httpClient?.GetAsync(url + "?X-Plex-Token=" + _plexApi?.ApiKey, cts.Token)!;

			//await GetDuplicates();
			
			return response.IsSuccessStatusCode;
		}
		catch (Exception)
		{
			return false;
		}
	}
	
	// public async Task<bool?> GetDuplicates()
	// {
	// 	List<Server> t = await PlexAccount?.Servers()!;
	// 	var libs = await t.First().Libraries();
	// 	var movieLib = (MovieLibrary)libs.First(x => x.Title == "Films");
	// 	var filters = movieLib.FilterFields; // movieLib.FilterFields.First().FilterFields.Select(x => x.Title)
	// 	
	//
	// 	return true;
	// }
}