using System.Xml.Linq;
using Europium.Dtos.Plex;
using Europium.Services.Apis.YggTorrent;

namespace Europium.Mappers.Plex;

public class PlexHistoryMapper : BaseMapper
{
    public async Task<List<PlexMediaHistory>> MapMediasHistory(Stream plexMediasHistory, List<PlexUser> plexUsers, List<PlexDevice> getDevicesResult)
    {
        var xml = await XDocument.LoadAsync(plexMediasHistory, LoadOptions.None, GetCancellationToken());
        return xml.Descendants("Video").Select(x => MapMediaHistory(x, plexUsers, getDevicesResult)).ToList();
    }

    private PlexMediaHistory MapMediaHistory(XElement video, List<PlexUser> plexUsers, List<PlexDevice> plexDevices)
    {
        int? id = video.Attribute("ratingKey") is not null ? (int)video.Attribute("ratingKey") : null;
        return new PlexMediaHistory
        {
            Id = id,
            Title = GetTitle(video),
            MediaType = GetMediaType(video),
            SeenAt = (long)video.Attribute("viewedAt"),
            User = GetUser(video, plexUsers),
            ParentId = GetKey(video, "grandparentKey") ?? id ?? 0,
            ThumbnailId = GetKey(video, "grandparentArt"),
            Device = GetDevice(video, plexDevices)
        };
    }

    private int? GetKey(XElement video, string key)
    {
        var text = ((string)video.Attribute(key) ?? "").Split('/').Last();
        return string.IsNullOrEmpty(text) ? null : int.Parse(text);
    }

    private string GetUser(XElement video, List<PlexUser> plexUsers)
    {
        return plexUsers.FirstOrDefault(x => x.Id == (int)video.Attribute("accountID"))?.Name ?? "";
    }

    private string GetDevice(XElement video, List<PlexDevice> plexDevices)
    {
        return plexDevices.FirstOrDefault(x => x.Id == (int)video.Attribute("deviceID"))?.Name ?? "";
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