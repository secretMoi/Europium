using System.Xml.Linq;
using Europium.Dtos.Plex;

namespace Europium.Mappers.Plex;

public class PlexSessionMapper
{
    public List<PlexPlayingMedia> MapPlayingMedias(XDocument xml)
    {
        return xml.Descendants("Video").Select(MapPlayingMedia).ToList();
    }

    private PlexPlayingMedia MapPlayingMedia(XElement videoElement)
    {
        return new PlexPlayingMedia
        {
            Id = (int?)videoElement.Attribute("grandparentRatingKey") ?? (int)videoElement.Attribute("ratingKey"),
            Title = GetTitle(videoElement),
            RemoteBitrate = (int)videoElement.Element("Session")!.Attribute("bandwidth"),
            IsRemote = (string)videoElement.Element("Session")?.Attribute("location") == "wan",
            IsPlaying = (string)videoElement.Element("Player")?.Attribute("state") == "playing",
            Progress = (int)videoElement.Attribute("viewOffset"),
            Duration = (int)videoElement.Attribute("duration"),
            Year = (int)videoElement.Attribute("year"),
            ThumbnailId = int.Parse(((string)videoElement.Attribute("thumb") ?? "").Split('/').Last()),
            UserName = (string)videoElement.Element("User")!.Attribute("title") ?? "",
            IsVideoTranscoding = IsTranscoding((string)videoElement.Element("TranscodeSession")!.Attribute("videoDecision") ?? ""),
            RemoteResolution = (string)videoElement.Element("Media")!.Attribute("videoResolution") ?? "",
            VideoCodec = (string)videoElement.Element("Media")!.Attribute("videoCodec") ?? "",
            IsAudioTranscoding = IsTranscoding((string)videoElement.Element("TranscodeSession")!.Attribute("audioDecision") ?? ""),
            AudioTitle = (string)GetAudio(videoElement).Attribute("displayTitle") ?? "",
        };
    }

    private XElement GetAudio(XElement videoElement)
    {
        return videoElement.Element("Media")!.Element("Part")!
            .Descendants()
            .First(x => x.Attributes("channels").Any());
    }

    private bool IsTranscoding(string isTranscoding)
    {
        return isTranscoding == "transcode";
    }

    private string GetTitle(XElement videoElement)
    {
        return (string)videoElement.Attribute("grandparentTitle") ??
               (string)videoElement.Attribute("title") ?? "";
    }
}