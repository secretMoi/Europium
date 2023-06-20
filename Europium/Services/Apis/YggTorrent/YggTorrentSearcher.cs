using Europium.Helpers.Extensions;
using Europium.Repositories;

namespace Europium.Services.Apis.YggTorrent;

public class YggTorrentSearcher
{
    private readonly YggTorrentRepository _yggTorrentRepository;

    public YggTorrentSearcher(YggTorrentRepository yggTorrentRepository)
    {
        _yggTorrentRepository = yggTorrentRepository;
    }
    
    public async Task<List<YggTorrentSearchDto>> SearchTorrent(string torrentName)
    {
        var htmlResponse = await _yggTorrentRepository.SearchTorrent(torrentName);

        if (htmlResponse.Contains("Aucun résultat"))
            throw new KeyNotFoundException();
        
        htmlResponse = htmlResponse.RemoveBefore("listing torrents");
        htmlResponse = htmlResponse.RemoveAfter("end table");
        htmlResponse = htmlResponse.RemoveBefore("<", false);
        htmlResponse = htmlResponse.RemoveAfterLast(">");

        var torrentsHtml = htmlResponse.Split("<tr>").Skip(1);

        var torrents = new List<YggTorrentSearchDto>();
        foreach (var torrentHtml in torrentsHtml)
        {
            var name = GetTorrentName(torrentHtml);
            
            if(SkipTorrent(name)) continue;
            
            var pageUrl = GetPageUrl(torrentHtml);
            var torrent = new YggTorrentSearchDto
            {
                Name = name,
                PageUrl = pageUrl,
                TorrentUrl = GetTorrentUrl(pageUrl),
                Size = GetTorrentSize(torrentHtml),
                Downloaded = GetTorrentDownloaded(torrentHtml),
                Seeders = GetTorrentSeeders(torrentHtml),
                Age = GetTorrentAge(torrentHtml),
                MediaQuality = GetTorrentQuality(name),
                MediaType = GetTorrentType(torrentHtml)
            };
            
            torrents.Add(torrent);
        }

        return torrents;
    }

    private MediaType GetTorrentType(string torrentHtml)
    {
        var type = torrentHtml.Split("</td>")[0];
        if (type.Contains("2183")) return MediaType.Movie;
        if (type.Contains("2184")) return MediaType.Serie;

        return MediaType.Unknown;
    }

    private MediaQuality GetTorrentQuality(string name)
    {
        if (name.Contains("720p")) return MediaQuality.HD;
        if (name.Contains("1080p")) return MediaQuality.FHD;
        if (name.Contains("2160p")) return MediaQuality.UHD;

        return MediaQuality.Unknown;
    }

    private bool SkipTorrent(string name)
    {
        var torrentName = name.ToLower();

        if(torrentName.Contains("vfq")) return true;
        if(!torrentName.Contains("1080") && !torrentName.Contains("2160")) return true;
        if(torrentName.Contains("french") && !torrentName.Contains("truefrench")) return true;

        return false;
    }
    
    private string GetTorrentName(string torrentHtml)
    {
        var name = "<" + torrentHtml.RemoveBefore("torrent_name");
        name = name.RemoveAfter("get_nfo") + ">";
        name = name.RemoveAllBetween('<', '>');
        name = name.Replace("\r", string.Empty);
        name = name.Replace("\n", string.Empty);
        name = name.Trim();
        return name;
    }

    private string GetPageUrl(string torrentHtml)
    {
        return torrentHtml
            .Split("href=\"")[2]
            .RemoveAfter("\"")
            .Replace("&amp;", "+");
    }

    private string GetTorrentUrl(string pageUrl)
    {
        var torrentId = pageUrl.Split("/").Last().RemoveAfter("-");

        return _yggTorrentRepository.GetDownloadTorrentUrl(torrentId);
    }

    private string GetTorrentSize(string torrentHtml)
    {
        return torrentHtml.Split("<td>")[5].RemoveAfter("<");
    }

    private string GetTorrentDownloaded(string torrentHtml)
    {
        return torrentHtml.Split("<td>")[6].RemoveAfter("<");
    }

    private string GetTorrentSeeders(string torrentHtml)
    {
        return torrentHtml.Split("<td>")[7].RemoveAfter("<");
    }

    private string GetTorrentAge(string torrentHtml)
    {
        return torrentHtml
            .Split("<td>")[4]
            .Split("</span>")[1]
            .RemoveAllBetween('<', '>')
            .Trim();
    }
}