using Europium.Repositories;

namespace Europium.Services.Apis.QBitTorrent;

public class QBitTorrentService
{
	private readonly QBitTorrentRepository _qBitTorrentRepository;

	public QBitTorrentService(QBitTorrentRepository qBitTorrentRepository)
	{
		_qBitTorrentRepository = qBitTorrentRepository;
	}

	public async Task<bool> IsUpAsync()
	{
		return await _qBitTorrentRepository.IsUpAsync();
	}

	public async Task<List<TorrentInfo>?> GetAllAsync()
	{
		return await _qBitTorrentRepository.GetAllAsync();
	}

	public async Task<bool> DeleteTorrentAsync(string torrentHash)
	{
		return await _qBitTorrentRepository.DeleteTorrentAsync(torrentHash);
	}

	public async Task<bool> AddTorrent(int torrentId)
	{
		return await _qBitTorrentRepository.AddTorrent(torrentId);
	}
}