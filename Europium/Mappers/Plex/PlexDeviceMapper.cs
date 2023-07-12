using System.Xml.Linq;
using Europium.Dtos.Plex;

namespace Europium.Mappers.Plex;

public class PlexDeviceMapper : BaseMapper
{
    public async Task<List<PlexDevice>> MapDevices(Stream plexDevices)
    {
        var xml = await XDocument.LoadAsync(plexDevices, LoadOptions.None, GetCancellationToken());
        return xml.Descendants("Device").Select(MapDevice).ToList();
    }

    private PlexDevice MapDevice(XElement device)
    {
        return new PlexDevice
        {
            Id = (int)device.Attribute("id"),
            Name = (string)device.Attribute("name") ?? "",
        };
    }
}