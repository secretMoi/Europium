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

	public async Task<List<PlexDuplicateDto>> GetDuplicates(PlexLibraryType libraryType, int libraryId)
	{
		return await _plexRepository.GetDuplicates(libraryType, libraryId);
	}

	public async Task<List<PlexLibraryDto>> GetLibraries()
	{
		return await _plexRepository.GetLibraries();
	}

	public async Task<bool> DeleteMedia(int mediaId, int fileId)
	{
		return await _plexRepository.DeleteMedia(mediaId, fileId);
	}
}