using System.Xml.Linq;
using Europium.Dtos.Plex;
using Europium.Services.Apis.YggTorrent;

namespace Europium.Mappers.Plex;

public class PlexHistoryMapper : BaseMapper
{
    public async Task<List<PlexMediaHistory>> MapMediasHistory(Stream plexMediasHistory, List<PlexUser> plexUsers)
    {
        var xml = await XDocument.LoadAsync(plexMediasHistory, LoadOptions.None, GetCancellationToken());
        return xml.Descendants("Video").Select(x => MapMediaHistory(x, plexUsers)).ToList();
    }

    private PlexMediaHistory MapMediaHistory(XElement video, List<PlexUser> plexUsers)
    {
        return new PlexMediaHistory
        {
            Id = video.Attribute("ratingKey") is not null ? (int)video.Attribute("ratingKey") : null,
            Title = GetTitle(video),
            MediaType = GetMediaType(video),
            SeenAt = (long)video.Attribute("viewedAt"),
            User = GetUser(video, plexUsers)
        };
    }

    private string GetUser(XElement video, List<PlexUser> plexUsers)
    {
        return plexUsers.FirstOrDefault(x => x.Id == (int)video.Attribute("accountID"))?.Name ?? "";
    }

    private string GetTitle(XElement video)
    {
        return (string)video.Attribute("grandparentTitle") ??
               (string)video.Attribute("title") ?? "";
    }

    private MediaType GetMediaType(XElement video)
    {
        return (string)video.Attribute("type") == "movie" ? MediaType.Movie : MediaType.Serie;
    }
}