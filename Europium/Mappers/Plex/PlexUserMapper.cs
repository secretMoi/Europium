using System.Xml.Linq;
using Europium.Dtos.Plex;

namespace Europium.Mappers.Plex;

public class PlexUserMapper : BaseMapper
{
    public async Task<List<PlexUser>> MapUsers(Stream userStream)
    {
        var xml = await XDocument.LoadAsync(userStream, LoadOptions.None, GetCancellationToken());
        return xml.Descendants("Account").Select(MapUser).ToList();
    }

    private PlexUser MapUser(XElement user)
    {
        return new PlexUser
        {
            Id = (int)user.Attribute("id"),
            Name = (string)user.Attribute("name") ?? "",
        };
    }
}