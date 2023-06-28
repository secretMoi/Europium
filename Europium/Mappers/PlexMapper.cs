using System.Xml.Linq;
using Europium.Dtos.Plex;

namespace Europium.Mappers;

public class PlexMapper
{
    public List<PlexDuplicateDto> MapSeriesDuplicates(XDocument xml)
    {
        var plexDuplicates = new List<PlexDuplicateDto>();
        foreach (var videoElement in xml.Descendants("Video"))
        {
            var plexDuplicate = new PlexDuplicateDto
            {
                Id = (int)videoElement.Attribute("ratingKey"),
                Title = (string)videoElement.Attribute("originalTitle") + " " + (string)videoElement.Attribute("parentTitle") + " " + (string)videoElement.Attribute("title"),
                PlexMedias = new List<PlexMediaDto>()
            };

            MapPlexMedias(videoElement, plexDuplicate.PlexMedias);

            plexDuplicates.Add(plexDuplicate);
        }

        return plexDuplicates;
    }

    public List<PlexDuplicateDto> MapMovieDuplicates(XDocument xml)
    {
        var plexDuplicates = new List<PlexDuplicateDto>();
        foreach (var videoElement in xml.Descendants("Video"))
        {
            var plexDuplicate = new PlexDuplicateDto
            {
                Id = (int)videoElement.Attribute("ratingKey"),
                Title = (string)videoElement.Attribute("title") ?? string.Empty,
                PlexMedias = new List<PlexMediaDto>()
            };

            MapPlexMedias(videoElement, plexDuplicate.PlexMedias);

            plexDuplicates.Add(plexDuplicate);
        }

        return plexDuplicates;
    }
    public List<PlexLibraryDto> MapLibraries(XDocument xml)
    {
        var plexDuplicates = new List<PlexLibraryDto>();
        foreach (var videoElement in xml.Descendants("Directory"))
        {
            plexDuplicates.Add(new PlexLibraryDto
            {
                Id = (int)videoElement.Attribute("key"),
                Title = (string)videoElement.Attribute("title") ?? string.Empty,
                Type = (string)videoElement.Attribute("type") == "movie" ? PlexLibraryType.Movie : PlexLibraryType.Serie
            });
        }

        return plexDuplicates;
    }

    private void MapPlexMedias(XElement element, List<PlexMediaDto> plexMedias)
    {
        plexMedias.AddRange(element.Descendants("Media").Select(MapPlexMedia));
    }

    private PlexMediaDto MapPlexMedia(XElement mediaElement)
    {
        return new PlexMediaDto
        {
            Id = (int)mediaElement.Attribute("id"),
            Bitrate = (int)mediaElement.Attribute("bitrate"),
            Resolution = (string)mediaElement.Attribute("videoResolution") ?? string.Empty,
            FilePath = (string)mediaElement.Element("Part")?.Attribute("file") ?? string.Empty,
            Size = (long)mediaElement.Element("Part")!.Attribute("size"),
        };
    }
}