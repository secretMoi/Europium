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
        htmlResponse = htmlResponse.RemoveBefore("listing torrents");
        htmlResponse = htmlResponse.RemoveAfter("end table");
        htmlResponse = htmlResponse.RemoveBefore("<", false);
        htmlResponse = htmlResponse.RemoveAfterLast(">");

        var torrentsHtml = htmlResponse.Split("<tr>").Skip(1);

        var torrents = new List<YggTorrentSearchDto>();
        foreach (var torrentHtml in torrentsHtml)
        {
            var pageUrl = GetPageUrl(torrentHtml);
            var torrent = new YggTorrentSearchDto
            {
                Name = GetTorrentName(torrentHtml),
                PageUrl = pageUrl,
                TorrentUrl = GetTorrentUrl(pageUrl),
                Size = GetTorrentSize(torrentHtml),
                Downloaded = GetTorrentDownloaded(torrentHtml),
                Seeders = GetTorrentSeeders(torrentHtml),
                Age = GetTorrentAge(torrentHtml),
            };
            
            torrents.Add(torrent);
        }

        return torrents;
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