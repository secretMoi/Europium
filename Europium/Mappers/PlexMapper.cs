using System.Xml.Linq;
using Europium.Dtos.Plex;

namespace Europium.Mappers;

public class PlexMapper
{
    public List<PlexDuplicateDto> MapDuplicates(XDocument xml)
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