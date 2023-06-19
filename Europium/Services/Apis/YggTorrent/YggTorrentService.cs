using Europium.Dtos;

namespace Europium.Services.Apis.YggTorrent;

public class YggTorrentService
{
    private readonly YggTorrentSearcher _searcher;
    private readonly YggTorrentRatioFetcher _ratioFetcher;

    public YggTorrentService(YggTorrentSearcher searcher, YggTorrentRatioFetcher ratioFetcher)
    {
        _searcher = searcher;
        _ratioFetcher = ratioFetcher;
    }

    public async Task<List<YggTorrentSearchDto>> SearchByName(string name)
    {
        return await _searcher.SearchTorrent(name);
    }

    public async Task<YggTorrentAccount> GetRatio()
    {
        return await _ratioFetcher.GetRatio();
    }
}