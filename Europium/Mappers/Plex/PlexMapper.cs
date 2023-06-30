using System.Xml.Linq;
using Europium.Dtos.Plex;

namespace Europium.Mappers.Plex;

public class PlexMapper
{
    public List<PlexDuplicate> MapDuplicates(XDocument xml, PlexLibraryType libraryType)
    {
        var plexDuplicates = new List<PlexDuplicate>();
        foreach (var videoElement in xml.Descendants("Video"))
        {
            var plexDuplicate = MapDuplicate(videoElement, libraryType);
            MapPlexMedias(videoElement, plexDuplicate);
            plexDuplicate.TotalSize = plexDuplicate.PlexMedias.Sum(x => x.Size);
            plexDuplicates.Add(plexDuplicate);
        }

        return plexDuplicates;
    }
    
    public List<PlexLibrary> MapLibraries(XDocument xml)
    {
        var plexDuplicates = new List<PlexLibrary>();
        foreach (var videoElement in xml.Descendants("Directory"))
        {
            plexDuplicates.Add(new PlexLibrary
            {
                Id = (int)videoElement.Attribute("key"),
                Title = (string)videoElement.Attribute("title") ?? string.Empty,
                Type = (string)videoElement.Attribute("type") == "movie" ? PlexLibraryType.Movie : PlexLibraryType.Serie
            });
        }

        return plexDuplicates;
    }

    private PlexDuplicate MapDuplicate(XElement videoElement, PlexLibraryType libraryType)
    {
        var thumbnail = libraryType == PlexLibraryType.Movie
            ? (string)videoElement.Attribute("thumb")
            : (string)videoElement.Attribute("grandparentThumb");
        
        return new PlexDuplicate
        {
            Id = (int)videoElement.Attribute("ratingKey"),
            Title = (string)videoElement.Attribute(libraryType == PlexLibraryType.Movie ? "title" : "grandparentTitle") ?? string.Empty,
            ParentId = libraryType == PlexLibraryType.Movie ? (int)videoElement.Attribute("ratingKey") : (int)videoElement.Attribute("grandparentRatingKey"),
            ThumbnailId = int.Parse(thumbnail!.Split('/').Last()),
            PlexMedias = new List<PlexMedia>()
        };
    }

    private void MapPlexMedias(XElement element, PlexDuplicate plexDuplicate)
    {
        plexDuplicate.PlexMedias.AddRange(element.Descendants("Media").Select(MapPlexMedia));
    }

    private PlexMedia MapPlexMedia(XElement mediaElement)
    {
        return new PlexMedia
        {
            Id = (int)mediaElement.Attribute("id"),
            Bitrate = (int)mediaElement.Attribute("bitrate"),
            Resolution = (string)mediaElement.Attribute("videoResolution") ?? string.Empty,
            FilePath = (string)mediaElement.Element("Part")?.Attribute("file") ?? string.Empty,
            VideoCodec = (string)mediaElement.Attribute("videoCodec") ?? string.Empty,
            Size = (long)mediaElement.Element("Part")!.Attribute("size")
        };
    }
}