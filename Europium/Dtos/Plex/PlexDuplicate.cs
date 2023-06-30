namespace Europium.Dtos.Plex;

public class PlexDuplicate
{
    public int Id { get; set; }
    public int ParentId { get; set; }
    public int ThumbnailId { get; set; }
    public string Title { get; set; }
    public long TotalSize { get; set; }

    public List<PlexMedia> PlexMedias { get; set; }
}