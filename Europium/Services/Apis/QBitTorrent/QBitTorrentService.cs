using Europium.Repositories;
using Europium.Services.Apis.YggTorrent;

namespace Europium.Services.Apis.QBitTorrent;

public class QBitTorrentService
{
	private readonly QBitTorrentRepository _qBitTorrentRepository;
	private readonly YggTorrentRepository _yggTorrentRepository;

	public QBitTorrentService(QBitTorrentRepository qBitTorrentRepository, YggTorrentRepository yggTorrentRepository)
	{
		_qBitTorrentRepository = qBitTorrentRepository;
		_yggTorrentRepository = yggTorrentRepository;
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

	public async Task AddTorrent(int torrentId, MediaType mediaType)
	{
		var streamContent = await _yggTorrentRepository.DownloadTorrentFile(torrentId);
		await _qBitTorrentRepository.AddTorrent(await streamContent.ReadAsByteArrayAsync(), torrentId + ".torrent", mediaType);
	}
}