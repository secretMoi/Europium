using Europium.Services.Apis.YggTorrent;

namespace Europium.Dtos.Plex;

public class PlexMediaHistory
{
    public int? Id { get; set; }
    public string User { get; set; }
    public string Device { get; set; }
    public string Title { get; set; }
    public MediaType MediaType { get; set; }
    public long SeenAt { get; set; }
    public int ParentId { get; set; }
    public int? ThumbnailId { get; set; }
}