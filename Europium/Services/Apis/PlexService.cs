using Europium.Dtos.Plex;
using Europium.Repositories;

namespace Europium.Services.Apis;

public class PlexService
{
	private readonly PlexRepository _plexRepository;

	public PlexService(PlexRepository plexRepository)
	{
		_plexRepository = plexRepository;
	}
	
	public async Task<bool?> IsUpAsync(string url)
	{
		return await _plexRepository.IsUpAsync(url);
	}

	public async Task<List<PlexDuplicateDto>> GetDuplicates(int libraryId)
	{
		return await _plexRepository.GetDuplicates(libraryId);
	}
}