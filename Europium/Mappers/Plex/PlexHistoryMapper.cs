using System.Xml.Linq;
using Europium.Dtos.Plex;
using Europium.Services.Apis.YggTorrent;

namespace Europium.Mappers.Plex;

public class PlexHistoryMapper
{
    public List<PlexMediaHistory> MapMediasHistory(XDocument xml)
    {
        return xml.Descendants("Video").Select(MapMediaHistory).ToList();
    }

    private PlexMediaHistory MapMediaHistory(XElement video)
    {
        return new PlexMediaHistory
        {
            Id = video.Attribute("ratingKey") is not null ? (int)video.Attribute("ratingKey") : null,
            Title = GetTitle(video),
            MediaType = GetMediaType(video),
            SeenAt = (long)video.Attribute("viewedAt"),
        };
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