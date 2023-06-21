using Europium.Helpers.Extensions;
using Europium.Mappers;
using Europium.Repositories;

namespace Europium.Services.Apis.YggTorrent;

public class YggTorrentSearcher
{
    private readonly YggTorrentRepository _yggTorrentRepository;
    private readonly SizeMapper _sizeMapper;
    private readonly YggMapper _yggMapper;

    public YggTorrentSearcher(YggTorrentRepository yggTorrentRepository, SizeMapper sizeMapper, YggMapper yggMapper)
    {
        _yggTorrentRepository = yggTorrentRepository;
        _sizeMapper = sizeMapper;
        _yggMapper = yggMapper;
    }

    public async Task<List<YggTorrentSearchDto>> SearchTorrent(string torrentName)
    {
        var pages = await _yggTorrentRepository.SearchTorrents(torrentName);

        if (pages.First().Contains("Aucun résultat"))
            throw new KeyNotFoundException();

        var torrents = new List<YggTorrentSearchDto>();
        foreach (var page in pages)
            ParsePage(page, torrents);

        return torrents;
    }

    private void ParsePage(string page, List<YggTorrentSearchDto> torrents)
    {
        page = page.RemoveBefore("listing torrents");
        page = page.RemoveAfter("end table");
        page = page.RemoveBefore("<", false);
        page = page.RemoveAfterLast(">");

        var torrentsHtml = page.Split("<tr>").Skip(1);

        foreach (var torrentHtml in torrentsHtml)
        {
            var htmlSplit = torrentHtml.Split("</td>");
            var name = GetTorrentName(htmlSplit[1]);
            var type = GetTorrentType(htmlSplit[0]);

            if (SkipTorrent(name, type)) continue;

            var pageUrl = GetPageUrl(htmlSplit[1]);
            var torrent = new YggTorrentSearchDto
            {
                Name = name,
                PageUrl = pageUrl,
                TorrentUrl = GetTorrentUrl(pageUrl),
                Size = GetTorrentSize(htmlSplit[5]),
                Downloaded = GetTorrentDownloaded(htmlSplit[6]),
                Seeders = GetTorrentSeeders(htmlSplit[7]),
                Age = GetTorrentAge(htmlSplit[4]),
                MediaQuality = GetTorrentQuality(name),
                MediaType = type
            };

            torrents.Add(torrent);
        }
    }

    private MediaType GetTorrentType(string type)
    {
        if (type.Contains("2183")) return MediaType.Movie;
        if (type.Contains("2184")) return MediaType.Serie;
        if (type.Contains("2178") || type.Contains("2179")) return MediaType.Anime;

        return MediaType.Unknown;
    }

    private MediaQuality GetTorrentQuality(string name)
    {
        name = name.ToLower();
        if (name.Contains("720")) return MediaQuality.HD;
        if (name.Contains("1080")) return MediaQuality.FHD;
        if (name.Contains("2160")) return MediaQuality.UHD;

        return MediaQuality.Unknown;
    }

    private bool SkipTorrent(string name, MediaType mediaType)
    {
        var torrentName = name.ToLower();

        if (torrentName.Contains("vfq")) return true;
        if (!torrentName.Contains("1080") && !torrentName.Contains("2160") && !torrentName.Contains("fhd") && !torrentName.Contains("uhd")) return true;
        if (torrentName.Contains("french") && !torrentName.Contains("truefrench")) return true;
        if (mediaType == MediaType.Unknown) return true;

        return false;
    }

    private string GetTorrentName(string torrentHtml)
    {
        return torrentHtml.RemoveAllBetween('<', '>')
            .Replace("\r", string.Empty)
            .Replace("\n", string.Empty)
            .Trim();
    }

    private string GetPageUrl(string torrentHtml)
    {
        return torrentHtml
            .Split("href=\"")[1]
            .RemoveAfter("\"")
            .Replace("&amp;", "+");
    }

    private string GetTorrentUrl(string pageUrl)
    {
        var torrentId = pageUrl.Split("/").Last().RemoveAfter("-");

        return _yggTorrentRepository.GetDownloadTorrentUrl(torrentId);
    }

    private long GetTorrentSize(string torrentHtml)
    {
        return _sizeMapper.ValueToByte(torrentHtml.RemoveBefore(">"));
    }

    private int GetTorrentDownloaded(string torrentHtml)
    {
        return int.Parse(torrentHtml.RemoveBefore(">"));
    }

    private int GetTorrentSeeders(string torrentHtml)
    {
        return int.Parse(torrentHtml.RemoveBefore(">"));
    }

    private long GetTorrentAge(string torrentHtml)
    {
        return _yggMapper.MapTorrentAge(
            torrentHtml.Split("</span>")[1].Trim()
        );
    }
}